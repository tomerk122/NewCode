namespace UserManagement.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters.")]
        public string Password { get; set; } = string.Empty;

        public bool Active { get; set; }
        public int? UserGroupId { get; set; }
        public UserData Data { get; set; } = new UserData();
    }


    public class UserData
    {
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must contain only digits.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters.")]
        public string LastName { get; set; } = string.Empty;

        public string CreationDate { get; set; } = string.Empty;
    }
}