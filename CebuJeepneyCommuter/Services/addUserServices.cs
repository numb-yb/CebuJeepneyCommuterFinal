using CebuJeepneyCommuter.Models;
using System.Text.Json;

namespace CebuJeepneyCommuter.Services
{
    public class UserService
    {
        private readonly string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "users.json");

        public async Task SaveUserAsync(User user)
        {
            try
            {
                List<User> users = new();

                if (File.Exists(filePath))
                {
                    string json = await File.ReadAllTextAsync(filePath);
                    users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                }

                users.Add(user);

                string newJson = JsonSerializer.Serialize(users);
                await File.WriteAllTextAsync(filePath, newJson);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save user: {ex.Message}", "OK");
            }
        }

        public async Task<User?> GetUserByLoginAsync(string email, string password)
        {
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

                return users.FirstOrDefault(u => u.Email == email && u.Password == password);
            }

            return null;
        }

        public async Task<bool> ValidateUserLoginAsync(string email, string password)
        {
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

                return users.Any(u => u.Email == email && u.Password == password);
            }

            return false;
        }

        public async Task SaveLoggedInUserAsync(User user)
        {
            string loggedInPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "current_user.json");

            string json = JsonSerializer.Serialize(user);
            await File.WriteAllTextAsync(loggedInPath, json);
        }

        public async Task<User?> GetLoggedInUserAsync()
        {
            string loggedInPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "current_user.json");

            if (File.Exists(loggedInPath))
            {
                string json = await File.ReadAllTextAsync(loggedInPath);
                return JsonSerializer.Deserialize<User>(json);
            }

            return null;
        }
        public async Task UpdateUserAsync(User updatedUser)
        {
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

                var index = users.FindIndex(u => u.Email == updatedUser.Email);
                if (index != -1)
                {
                    users[index] = updatedUser;
                    string newJson = JsonSerializer.Serialize(users);
                    await File.WriteAllTextAsync(filePath, newJson);

                    // Also update the logged-in user file
                    await SaveLoggedInUserAsync(updatedUser);
                }
            }
        }
    }
}
