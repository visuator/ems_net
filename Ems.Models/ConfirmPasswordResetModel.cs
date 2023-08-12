namespace Ems.Models;

public class ConfirmPasswordResetModel
{
    public string PasswordResetToken { get; set; }
    public DateTime RequestedAt { get; set; }
}