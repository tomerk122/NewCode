using System.ComponentModel;
namespace UserManagement.Api.Models.DTOS
{

    /// <summary>
    ///     this function is for the searching user by First Name & Last Name
    /// </summary>
    public class SearchUserDto
    {
        [DefaultValue(null)]
        public string? FirstName { get; set; }

        [DefaultValue(null)]
        public string? LastName { get; set; }
    }
}
