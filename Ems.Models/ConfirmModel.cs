namespace Ems.Models;

public class ConfirmModel
{
    public string ConfirmationToken { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
}