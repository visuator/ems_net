namespace Ems.Models.Dtos;

public class ClassPeriodDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public TimeSpan StartingAt { get; set; }
    public TimeSpan EndingAt { get; set; }
    public string Name { get; set; }
}