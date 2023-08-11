namespace Ems.Models;

public class ConfirmPasswordResetModel
{
    public string PasswordResetToken { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
}