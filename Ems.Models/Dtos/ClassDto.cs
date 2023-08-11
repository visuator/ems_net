using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class ClassDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Guid? ClassVersionId { get; set; }
    public ClassVersionDto? ClassVersion { get; set; }
    public Guid? TemplateId { get; set; }
    public ClassDto? Template { get; set; }
    public Quarter? Quarter { get; set; }
    public DayOfWeek? Day { get; set; }
    public Guid? GroupId { get; set; }
    public GroupDto? Group { get; set; }
    public Guid? ClassPeriodId { get; set; }
    public ClassPeriodDto? ClassPeriod { get; set; }
    public Guid LecturerId { get; set; }
    public LecturerDto Lecturer { get; set; }
    public Guid LessonId { get; set; }
    public LessonDto Lesson { get; set; }
    public Guid ClassroomId { get; set; }
    public ClassroomDto Classroom { get; set; }
    public ClassType Type { get; set; }
    public string? Link { get; set; }
    public DateTimeOffset? StartingAt { get; set; }
    public DateTimeOffset? EndingAt { get; set; }
}