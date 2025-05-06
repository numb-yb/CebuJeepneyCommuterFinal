using CebuJeepneyCommuter.Models;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CebuJeepneyCommuter.Services
{
    public class UserService
    {
        private readonly string filePath = Path.Combine(@"C:\Users\chris\OneDrive\Documents\GitHub\CebuJeepneyCommuterFinal", "users.json");

        public async Task SaveUserAsync(User user)
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

        // Validate if the user login credentials match any of the users in the file
        public async Task<bool> ValidateUserLoginAsync(string email, string password)
        {
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

                // Check if any user has the matching email and password
                var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);

                return user != null; // Return true if user is found, otherwise false
            }
            return false; // If file doesn't exist, return false
        }
    }
}
