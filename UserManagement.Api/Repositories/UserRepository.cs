using UserManagement.Api.Models;
using UserManagement.Api.Models.DTOS;

namespace UserManagement.Api.Repositories
{
    public static class UserRepository
    {
        private const string FilePath = "App_Data/Users.json"; // נתיב הקובץ
        private static List<User> _cachedUsers;
        // loading the json evey time is not efficient, so we will load it once and cache it
        // in the static constructor
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        static UserRepository()
        {
            // Initialize cache on first access
            _cachedUsers = JsonUserStorage.LoadUsers();
        }

        public static User? GetUserById(int userId) // can be null, so in the function we will check if it is null
        {
            return _cachedUsers.FirstOrDefault(user => user.UserId == userId);
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
            _cachedUsers = JsonUserStorage.LoadUsers();
        }


        public static void SaveUsers()
        {
            JsonUserStorage.SaveUsers(_cachedUsers);
        }

        public static bool CheckUserName(string userName) // if the username exists in the list
        {

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


                JsonUserStorage.SaveUsers(_cachedUsers);
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
            catch (Exception ex) {
                throw new Exception("Error generating new user ID: " + ex.Message);
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

        public static List<User> SearchUsersByFirstOrLastName(SearchUserDto searchCriteria)
        {
            return _cachedUsers.Where(user =>
                (string.IsNullOrEmpty(searchCriteria.FirstName) ||
                 user.Data?.FirstName?.Equals(searchCriteria.FirstName, StringComparison.OrdinalIgnoreCase) == true) &&
                (string.IsNullOrEmpty(searchCriteria.LastName) ||
                 user.Data?.LastName?.Equals(searchCriteria.LastName, StringComparison.OrdinalIgnoreCase) == true)
            ).ToList();
        }


    }
}
