using Microsoft.AspNetCore.Mvc;
using UserManagement.Entites;
using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Controllers
{
    public class TestController : Controller
    {
        /*
         in this controller we are using the IItemsRepo 
         interface to create a new item and then retrieve all items from the repository.
         */
        private readonly IItemsRepo r1;
        private readonly Manager _manager;

        public TestController(IItemsRepo r1,Manager manager) // Constructor injection is used to inject the IItemsRepo dependency into the TestController.
        {
            this.r1 = r1;

            this._manager = manager;
        }
        public async Task<IActionResult> Index()
        
        {
            _manager.Print();
          

            ViewBag.Message = "Hello from TestController1!";
          
            // Create a new item

            await r1.CreateAsync(new Item
            {
                Id = Guid.NewGuid(),
                Name = "Test Item",
                Description = "This is a test item.",
                Price = 9.99m,
                CreatedDate = DateTime.UtcNow
            });

            await r1.CrearteEmailAsync(new Email
            {
                Password = "password",
                Port = 25,
                SmtpServer = "smtp.example.com",
                Username= "username"

            });
           var results=await r1.GetAllAsync();

            return View(results);

        }
       

        public IActionResult Check()
        {
            ViewBag.Message = "Hello from TestController2!";
            return View("~/Views/User/Index.cshtml");

        }
    }
}
