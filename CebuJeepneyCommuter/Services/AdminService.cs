using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using CebuJeepneyCommuter.Models;

namespace CebuJeepneyCommuter.Services
{
    public class AdminService
    {
        private const string FilePath = "users.json";

        public async Task<List<User>> GetAllUsersAsync()
        {
            if (!File.Exists(FilePath))
                return new List<User>();

            string json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var users = await GetAllUsersAsync();
            return users.Find(u => u.Email == email);
        }

        public async Task UpdateUserAsync(User updatedUser)
        {
            var users = await GetAllUsersAsync();
            int index = users.FindIndex(u => u.Email == updatedUser.Email);
            if (index != -1)
            {
                users[index] = updatedUser;
                await SaveUsersAsync(users);
            }
        }

        public async Task AddUserAsync(User user)
        {
            var users = await GetAllUsersAsync();
            users.Add(user);
            await SaveUsersAsync(users);
        }

        public async Task DeleteUserAsync(string email)
        {
            var users = await GetAllUsersAsync();
            users.RemoveAll(u => u.Email == email);
            await SaveUsersAsync(users);
        }

        private async Task SaveUsersAsync(List<User> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json, Encoding.UTF8);
        }
    }
}
