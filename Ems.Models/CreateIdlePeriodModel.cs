namespace Ems.Models;

public class CreateIdlePeriodModel
{
    public Guid? GroupId { get; set; }
    public DateTime StartingAt { get; set; }
    public DateTime EndingAt { get; set; }
}