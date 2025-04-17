using UserManagement.Models;
using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace UserManagement.Repositories
{
    public static class UserRepository
    {
        private const string FilePath = "App_Data/Users.json"; // נתיב הקובץ
        private static List<User> _cachedUsers;
        // loading the json evey time is not efficient, so we will load it once and cache it
        // in the static constructor

        static UserRepository()
        {
            // Initialize cache on first access
            _cachedUsers = JsonUserStorage.LoadUsers();
        }

        public static User GetUserById(int userId)
        {
            return _cachedUsers.FirstOrDefault(user => user.UserId == userId);
        }


        public static List<User> GetCachedUsers()
        {
            return _cachedUsers;
        }

        public static List<User> LoadUsersFromFile()
        {
            if (!File.Exists(FilePath))
            {
                return new List<User>();
            }
            var jsonData = File.ReadAllText(FilePath);

            var wrapper = JsonConvert.DeserializeObject<UserWrapper>(jsonData);
            return wrapper?.Users ?? new List<User>();
        }
        private class UserWrapper
        {
            public List<User> Users { get; set; }
        }

        public static void RefreshCache()
        {
            _cachedUsers = LoadUsersFromFile();
        }


        public static void SaveUsers()
        {
            JsonUserStorage.SaveUsers(_cachedUsers);
        }
        public static void DeleteUser(int userId)
        {
            var userToDelete = _cachedUsers.FirstOrDefault(user => user.UserId == userId);
            if (userToDelete != null)
            {
                _cachedUsers.Remove(userToDelete);
                SaveUsers(); // Update file after cache change
            }
        }
        public static bool CheckUserName(string userName)
        {

            return _cachedUsers.Any(user =>
              user.UserName != null &&
                user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));


        }
        public static void AddUser(User newUser)
        {
            if (CheckUserName(newUser.UserName))
            {
                throw new Exception("unique User Name!");
            }
            newUser.UserId = _cachedUsers.Any() ? _cachedUsers.Max(u => u.UserId) + 1 : 1; 
            // giving the new user a unique ID (from the max ID in the list)
            _cachedUsers.Add(newUser);
            SaveUsers();
        }

        public static void UpdateUser(User updatedUser)
        {
            var users = GetCachedUsers();
            if (updatedUser.UserId != users.FirstOrDefault(myUser => myUser.UserId == updatedUser.UserId)?.UserId)
            {
                if (CheckUserName(updatedUser.UserName))
                {
                    throw new Exception("unique User Name needed");
                }
            }

            var existingUser = users.FirstOrDefault(myUsers => myUsers.UserId == updatedUser.UserId);
            if (existingUser != null)
            {
                int index = _cachedUsers.IndexOf(existingUser);
                _cachedUsers[index] = updatedUser;
                SaveUsers();
            }
            else
            {
                throw new Exception("User not Found!");
            }
        }
        public static List<User> GetUsersByStatus(bool isActive)
        {
            return _cachedUsers
                .Where(user => user.Active == isActive)
                .ToList();
        }
        public static List<User> SearchUsers(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<User>();
            }

            return _cachedUsers.Where(user =>
                (!string.IsNullOrEmpty(user.UserName) && user.UserName.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(user.Data?.Email) && user.Data.Email.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(user.Data?.Phone) && user.Data.Phone.Contains(query))
            ).ToList();
        }


    }
}