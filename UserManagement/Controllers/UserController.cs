using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly Email _emailService;
        #region Load Users & index

        public UserController(Email emailService)
        {

            Console.WriteLine(  "Im from Controller");
            _emailService = emailService;
        }
        public IActionResult Index()
        {
           
            try
            {
                _emailService.Send("RWER","rwerwe","REWREW");
                var users = UserRepository.GetCachedUsers();
                if (!users.Any())
                {
                    SetErrorMessage("No users found.");
                }
                return View(users);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"Error retrieving users: {ex.Message}");
                return View(new List<User>());
            }
        }



        #endregion


        #region Filter Users functions
        // this function is used to load user by filtering by Active status
        public IActionResult FilterByStatus(bool isActive)
        {
            try
            {
                var filteredResult = UserRepository.GetUsersByStatus(isActive);

                if (!filteredResult.Any())
                {
                    SetErrorMessage("No users match the selected status.");
                }

                return View("Index", filteredResult);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"Error filtering users: {ex.Message}");
                return View("Index", new List<User>());
            }
        }






        // in this function we are searching users by a string, it could be a name or email or phone.
        public IActionResult SearchByCustomFilter(string UserData)
        {
            try
            {
                if(string.IsNullOrEmpty(UserData))
                {
                    SetErrorMessage("Please enter a valid search query.");
                    return View("Index", new List<User>());
                }
                var filteredResult = UserRepository.SearchUsers(UserData);

                if (!filteredResult.Any())
                {
                    SetErrorMessage("No users match the search term.");
                }

                return View("Index", filteredResult);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"Error searching users: {ex.Message}");
                return View("Index", new List<User>());
            }
        }


        #endregion



        #region Delete
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                UserRepository.DeleteUser(userId);
                SetSuccessMessage($"User with ID {userId} was deleted successfully.");
            }
            catch (Exception ex)
            {
                SetErrorMessage($"Error deleting user: {ex.Message}");
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region Create & view

        [BindProperty]
        public User newUser { get; set; }= new User();

        public IActionResult Create()
        {
        
            return View(newUser);
        }


        [HttpPost]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View("Create", newUser);
            }
            try
            {
                UserRepository.AddUser(newUser);
                SetSuccessMessage("User created successfully!");
            }
            catch (Exception ex)
            {
                SetErrorMessage($"Error created user: you need a {ex.Message}");
            }
            
            return RedirectToAction("Index");
        }

        #endregion



        #region Update Users

        [BindProperty]
        public User UserToUpdate { get; set; } = new User();

        public IActionResult Update()
        {
            return View();
        }
        public IActionResult UpdateUser(int userId)
        {
            try
            {
                var userToUpdate = UserRepository.GetUserById(userId);
                if (userToUpdate != null)
                {
                    return View("Update", userToUpdate);
                }

                SetErrorMessage("User not found.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetErrorMessage($"Error retrieving user for update: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdatePost()
        {
            if (!ModelState.IsValid)
            {
                return View("Update", UserToUpdate);
            }

            try
            {
                UserRepository.UpdateUser(UserToUpdate);
                SetSuccessMessage("User Updated successfully!");
            }
            catch (Exception ex)
            {
                SetErrorMessage($"Error Updated user: {ex.Message}");

            }

            return RedirectToAction("Index");
        }


        #endregion
        private void SetSuccessMessage(string message) => TempData["SuccessMessage"] = message;
        private void SetErrorMessage(string message) => TempData["ErrorMessage"] = message;


    }


}