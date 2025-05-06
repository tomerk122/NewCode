using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using UserManagement.Api.Middleware.JWT;
using UserManagement.Api.Models;
using UserManagement.Api.Models.DTOS;
using UserManagement.Api.Models.DTOS.DtoValidator;
using UserManagement.Api.Repositories;
namespace UserManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        [HttpGet("GetAllUsers")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                var users = UserRepository.GetCachedUsers();
                if (!users.Any())
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving users: {ex.Message}");
            }
        }


        [HttpGet("{userId}")]
        public ActionResult<User> GetUserById(int userId)
        {
            try
            {
                var user = UserRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching user by ID: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("TestTo")]
        public ActionResult Test([FromBody] string test)
        {
            try
            {
               return CreatedAtAction(nameof(GetUserById), new { userId = 1 }, test);
               

            }
            catch (Exception ex)
            {
                return BadRequest($"Error in test: {ex.Message}");
            }
        }


        [HttpPost("AddUser")]
        public ActionResult CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                var validtionErrors = DtoValidator.ValidateCreateUserDto(userDto);
                if (validtionErrors.Count > 0)
                {
                    return BadRequest($"Validation failed: {string.Join(", ", validtionErrors)}");
                }

                var newUser = ConvertToUser(userDto);
                UserRepository.AddUser(newUser);

                return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, newUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user: {ex.Message}");
            }
        }


        [HttpGet("SearchUser")]
        public ActionResult<User> SearchUser([FromQuery] SearchUserDto searchUserDto)
        {
            try
            {
                var validtionErrors = DtoValidator.ValidateSearchUserDto(searchUserDto);
                if (validtionErrors.Count > 0)
                {
                    return BadRequest($"Validation failed: {string.Join(", ", validtionErrors)}");
                }

                var users = UserRepository.SearchUsersByFirstOrLastName(searchUserDto);
                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error searching users: {ex.Message}");
            }

        }

        [HttpPost("GetToken")]
        [AllowAnonymous]
        public ActionResult GenerateToken([FromBody] Credentials ManagerCred)
        {
            try
            {
                var validationErrors = DtoValidator.ValidateCredentialsDto(ManagerCred);
                if (validationErrors.Count > 0)
                {
                    return BadRequest($"Validation failed: {string.Join(", ", validationErrors)}");
                }
                var credentials = UserRepository.AuthenticateUser(ManagerCred.UserName, ManagerCred.Password, ManagerCred.Company);
                if (!credentials)
                {
                    return Unauthorized("Invalid credentials.");
                }

                var token = JwtTokenGenerator.GenerateToken(ManagerCred);
                return Ok(new { Token = string.Join(" ","Bearer",token) });
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating token: {ex.Message}");
            }

        }






        #region Private Methods


        private User ConvertToUser(CreateUserDto dto)
        {
            // values are not null, so we can use the null-forgiving operator (!)
            return new User
            {
                UserName = dto.UserName!,
                Password = dto.Password!,
                Active = dto.Active,
                UserGroupId = dto.UserGroupId ?? 0,
                Data = new UserData
                {
                    Phone = dto.Phone!,
                    Email = dto.Email!,
                    FirstName = dto.FirstName!,
                    LastName = dto.LastName!,
                    CreationDate = DateTime.Now.ToString("yyyy-MM-dd")
                },
            };
        }



        #endregion

    }


  
}