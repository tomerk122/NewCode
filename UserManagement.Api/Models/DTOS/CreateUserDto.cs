using System.ComponentModel;

public class CreateUserDto
{
    [DefaultValue(null)]
    public string? UserName { get; set; }

    [DefaultValue(null)]
    public string? Password { get; set; }

    [DefaultValue(false)]
    public bool Active { get; set; }

    [DefaultValue(null)]
    public string? Email { get; set; }

    [DefaultValue(null)]
    public string? Phone { get; set; }

    [DefaultValue(null)]
    public string? FirstName { get; set; }

    [DefaultValue(null)]
    public string? LastName { get; set; }

    [DefaultValue(0)]
    public int? UserGroupId { get; set; }

  
}
