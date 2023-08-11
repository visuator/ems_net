namespace Ems.Models;

public class PasswordModel
{
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}