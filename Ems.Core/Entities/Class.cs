using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class Class : EntityBase, ISingleKeyEntity
{
    public Guid? ClassVersionId { get; set; }
    public ClassVersion? ClassVersion { get; set; }
    public Guid? TemplateId { get; set; }
    public Class? Template { get; set; }
    public Quarter? Quarter { get; set; }
    public DayOfWeek? Day { get; set; }
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    public Guid? ClassPeriodId { get; set; }
    public ClassPeriod? ClassPeriod { get; set; }
    public required Guid LecturerId { get; set; }
    public required Lecturer Lecturer { get; set; }
    public required Guid LessonId { get; set; }
    public required Lesson Lesson { get; set; }
    public required Guid ClassroomId { get; set; }
    public required Classroom Classroom { get; set; }
    public required ClassType Type { get; set; }
    public string? Link { get; set; }
    public DateTime? StartingAt { get; set; }
    public DateTime? EndingAt { get; set; }
    public required Guid Id { get; set; }
}