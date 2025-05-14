using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuJeepneyCommuter.Models;

namespace CebuJeepneyCommuter.Services
{
    public class AdminService
    {
        private const string AdminFilePath = "admin.json.txt";
        private const string UserFilePath = "users.json"; // Assuming users are stored in a separate file

        // Existing methods for Admin operations
        public async Task<List<Admin>> GetAllAdminsAsync()
        {
            if (!File.Exists(AdminFilePath))
                return new List<Admin>();

            string json = await File.ReadAllTextAsync(AdminFilePath);
            return JsonSerializer.Deserialize<List<Admin>>(json) ?? new List<Admin>();
        }

        public async Task<Admin?> GetAdminByEmailAndPasswordAsync(string email, string password)
        {
            var admins = await GetAllAdminsAsync();
            return admins.Find(a => a.Email == email && a.Password == password);
        }

        // New methods for User operations

        // Get all users from users.json
        public async Task<List<User>> GetAllUsersAsync()
        {
            if (!File.Exists(UserFilePath))
                return new List<User>();

            string json = await File.ReadAllTextAsync(UserFilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        // Add a new user to users.json
        public async Task AddUserAsync(User user)
        {
            var users = await GetAllUsersAsync();
            users.Add(user);
            await SaveUsersAsync(users);
        }

        // Delete a user from users.json based on their email
        public async Task DeleteUserAsync(string email)
        {
            var users = await GetAllUsersAsync();
            var userToDelete = users.FirstOrDefault(u => u.Email == email);
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                await SaveUsersAsync(users);
            }
        }

        // Update an existing user in users.json
        public async Task UpdateUserAsync(User updatedUser)
        {
            var users = await GetAllUsersAsync();
            var userIndex = users.FindIndex(u => u.Email == updatedUser.Email);
            if (userIndex != -1)
            {
                users[userIndex] = updatedUser;
                await SaveUsersAsync(users);
            }
        }

        // Helper method to save the updated list of users back to users.json
        private async Task SaveUsersAsync(List<User> users)
        {
            string json = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(UserFilePath, json);
        }
    }
}
