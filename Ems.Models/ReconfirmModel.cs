namespace Ems.Models;

public class ReconfirmModel
{
    public string Email { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
}