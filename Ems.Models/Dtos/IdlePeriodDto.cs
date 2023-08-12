namespace Ems.Models.Dtos;

public class IdlePeriodDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Guid? GroupId { get; set; }
    public GroupDto? Group { get; set; }
    public DateTime StartingAt { get; set; }
    public DateTime EndingAt { get; set; }
}