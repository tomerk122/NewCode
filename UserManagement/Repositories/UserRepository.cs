using UserManagement.Models;
using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Repositories
{
    public static class UserRepository
    {
        private static List<User> _cachedUsers;
        // loading the json evey time is not efficient, so we will load it once and cache it
        // in the static constructor

        static UserRepository()
        {
            // Initialize cache on first access
            _cachedUsers = JsonUserStorage.LoadUsers();
        }

        public static User? GetUserById(int userId) // can be null, so in the function we will check if it is null
        {
            try
            {
                return _cachedUsers.FirstOrDefault(user => user.UserId == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by ID {userId}: {ex.Message}");
                return null;
            }

        }


        public static List<User> GetCachedUsers()
        {
            return _cachedUsers;
        }

      

        public static void RefreshCache()
        {
            try
            {
                _cachedUsers = JsonUserStorage.LoadUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing user cache: {ex.Message}");
            }
        }


        public static void SaveUsers()
        {
            try
            {
                JsonUserStorage.SaveUsers(_cachedUsers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
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
        public static bool CheckUserName(string userName) // if the username exists in the list
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("User name is required!");
            }
            return _cachedUsers.Any(user =>
              user.UserName != null &&
                user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));


        }
        public static void AddUser(User newUser)
        {
            try
            {
                if (CheckUserName(newUser.UserName))
                {
                    throw new Exception("unique User Name!");
                }
                newUser.UserId = GenerateNewUserId();
                // giving the new user a unique ID (from the max ID in the list)
                _cachedUsers.Add(newUser);
                SaveUsers();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding user: " + ex.Message);
            }
        }
        public static int GenerateNewUserId()
        {

            try
            {
                return _cachedUsers.Any() ? _cachedUsers.Max(u => u.UserId) + 1 : 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating new user ID: " + ex.Message);
            }

        }

        public static void UpdateUser(User updatedUser)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error updating user: " + ex.Message);
            }
        }
        public static List<User> GetUsersByStatus(bool isActive)
        {
            try
            {
                return _cachedUsers
                    .Where(user => user.Active == isActive)
                    .ToList();
            } catch (Exception ex) {
                throw new Exception("Error filtering users by status: " + ex.Message);
            }
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