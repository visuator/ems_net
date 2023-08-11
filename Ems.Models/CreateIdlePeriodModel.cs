namespace Ems.Models;

public class CreateIdlePeriodModel
{
    public Guid? GroupId { get; set; }
    public DateTimeOffset StartingAt { get; set; }
    public DateTimeOffset EndingAt { get; set; }
}