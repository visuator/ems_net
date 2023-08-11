using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("classes", Schema = Schemas.Main)]
public class Class : EntityBase, ISingleKeyEntity
{
    [Column("class_version_id")] public Guid? ClassVersionId { get; set; }

    public ClassVersion? ClassVersion { get; set; }

    [Column("template_id")] public Guid? TemplateId { get; set; }

    public Class? Template { get; set; }

    [Column("quarter")] public Quarter? Quarter { get; set; }

    [Column("day")] public DayOfWeek? Day { get; set; }

    [Column("group_id")] public Guid? GroupId { get; set; }

    public Group? Group { get; set; }

    [Column("class_period_id")] public Guid? ClassPeriodId { get; set; }

    public ClassPeriod? ClassPeriod { get; set; }

    [Column("lecturer_id")] public Guid LecturerId { get; set; }

    public Lecturer Lecturer { get; set; }

    [Column("lesson_id")] public Guid LessonId { get; set; }

    public Lesson Lesson { get; set; }

    [Column("classroom_id")] public Guid ClassroomId { get; set; }

    public Classroom Classroom { get; set; }

    [Column("type")] public ClassType Type { get; set; }

    [Column("link")] public string? Link { get; set; }

    [Column("starting_at")] public DateTimeOffset? StartingAt { get; set; }

    [Column("ending_at")] public DateTimeOffset? EndingAt { get; set; }

    [Column("id")] public Guid Id { get; set; }
}