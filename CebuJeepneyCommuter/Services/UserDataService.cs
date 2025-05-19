using CebuJeepneyCommuter.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CebuJeepneyCommuter.Services
{
    public static class UserDataService
    {
        private static readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "users.json");

        public static async Task<List<User>> GetAllUsersAsync()
        {
            if (!File.Exists(filePath))
                return new List<User>();

            using var stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<List<User>>(stream) ?? new List<User>();
        }

        public static async Task SaveUsersAsync(List<User> users)
        {
            using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, users);
        }

        public static async Task DeleteUserAsync(User userToDelete)
        {
            var users = await GetAllUsersAsync();
            users.RemoveAll(u => u.Id == userToDelete.Id);
            await SaveUsersAsync(users);
        }


        public static async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var users = await UserDataService.GetAllUsersAsync();
            return users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public static async Task AddUserAsync(User newUser)
        {
            var users = await GetAllUsersAsync();

            // Assign a new unique ID
            newUser.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

            users.Add(newUser);
            await SaveUsersAsync(users);
        }
        // ✅ New: Update password using user ID
        public static async Task UpdateUserPasswordAsync(int userId, string newPassword)
        {
            var users = await GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Password = newPassword;
                await SaveUsersAsync(users);
            }
        }

        // ✅ New: Update password using email
        public static async Task UpdateUserPasswordByEmailAsync(string email, string newPassword)
        {
            var users = await GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Password = newPassword;
                await SaveUsersAsync(users);
            }
        }
    }
}
