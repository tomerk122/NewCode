using UserManagement.Api.Models;
using UserManagement.Api.Models.DTOS;

namespace UserManagement.Api.Repositories
{
    public static class UserRepository
    {

        private static List<User> _cachedUsers;
        private static List<Credentials> _cachedCredentials;

        // loading the json evey time is not efficient, so we will load it once and cache it
        // in the static constructor
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        static UserRepository()
        {
            // Initialize cache on first access
            _cachedUsers = JsonUserStorage.LoadUsers();
            _cachedCredentials = JsonCredentialsManager.LoadCredentials();
        }

        public static User? GetUserById(int userId) // can be null, so in the function we will check if it is null
        {
            try
            {
                return _cachedUsers?.FirstOrDefault(user => user.UserId == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by ID {userId}: {ex.Message}");
                return null;
            }
        }


        public static List<User> GetCachedUsers()
        {

            _lock.EnterReadLock();
            try
            {
                return _cachedUsers;
            }
            finally
            {
                _lock.ExitReadLock();
            }
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
        public static void RefreshCredentialsCache()
        {
            try
            {
                _cachedCredentials = JsonCredentialsManager.LoadCredentials();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing credentials cache: {ex.Message}");
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
            _lock.EnterWriteLock();
            try
            {
                if (CheckUserName(newUser.UserName))
                {
                    throw new Exception("Unique User Name is required!");
                }

                newUser.UserId = GenerateNewUserId();
                _cachedUsers.Add(newUser);


                //JsonUserStorage.SaveUsers(_cachedUsers);
                SaveUsers();
                RefreshCache();
            }
            finally
            {
                _lock.ExitWriteLock();
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


        public static List<User> SearchUsersByFirstOrLastName(SearchUserDto searchCriteria)
        {
            return _cachedUsers.Where(user =>
                (string.IsNullOrEmpty(searchCriteria.FirstName) ||
                 user.Data?.FirstName?.Equals(searchCriteria.FirstName, StringComparison.OrdinalIgnoreCase) == true) &&
                (string.IsNullOrEmpty(searchCriteria.LastName) ||
                 user.Data?.LastName?.Equals(searchCriteria.LastName, StringComparison.OrdinalIgnoreCase) == true)
            ).ToList();
        }



        #region Authentication for API users
        public static bool AuthenticateUser(string userName, string password, string company)
        {
            RefreshCredentialsCache(); // to make sure we have the latest credentials
            var credentials = _cachedCredentials;

            var userExists = credentials.Any(cred =>
                cred.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) &&
                cred.Password.Equals(password) &&
                cred.Company.Equals(company, StringComparison.OrdinalIgnoreCase));

            return userExists;
        }
        #endregion

    }
}