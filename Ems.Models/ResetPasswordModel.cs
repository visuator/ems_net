namespace Ems.Models;

public class ResetPasswordModel
{
    public string Email { get; set; }
    public DateTime RequestedAt { get; set; }
}