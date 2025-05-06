using Microsoft.AspNetCore.Mvc;
using UserManagement.Entites;
using UserManagement.Repositories;

namespace UserManagement.Controllers
{
    public class TestController : Controller
    {
        private readonly IItemsRepo r1;

        public TestController(IItemsRepo r1)
        {
            this.r1 = r1;
        }
        public async Task<IActionResult> Index()
        
        {

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
