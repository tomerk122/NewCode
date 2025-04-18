namespace UserManagement.Api.Models.DTOS
{

    /// <summary>
    ///     this class is for the credentials of the Management System who can access the system
    /// </summary>
    public class Credentials
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
