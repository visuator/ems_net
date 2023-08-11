namespace Ems.Models.Dtos;

public class IdlePeriodDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Guid? GroupId { get; set; }
    public GroupDto? Group { get; set; }
    public DateTimeOffset StartingAt { get; set; }
    public DateTimeOffset EndingAt { get; set; }
}