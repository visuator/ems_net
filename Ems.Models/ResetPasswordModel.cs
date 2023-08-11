namespace Ems.Models;

public class ResetPasswordModel
{
    public string Email { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
}