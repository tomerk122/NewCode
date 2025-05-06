using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost("Create")]
        public IActionResult Test([FromQuery] UserName myuser)
        {
            if(myuser == null)
            {
                return BadRequest("User data is null");
            }
            if (string.IsNullOrEmpty(myuser.FirstName) || string.IsNullOrEmpty(myuser.LastName) || string.IsNullOrEmpty(myuser.Email))
            {
                return BadRequest("User data is incomplete");
            }
            // Here you can add logic to save the user data to a database or perform other actions
            // For now, we'll just return a success message
            return Ok($"User {myuser.FirstName} {myuser.LastName} with email {myuser.Email} created successfully.");
        }
    }

    public class UserName
    {
        private string firstName;
        private string lastName;
        private string email;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }

        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public UserName(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public UserName() // Default constructor
        {
            Console.WriteLine("Building");
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }
    }
}
