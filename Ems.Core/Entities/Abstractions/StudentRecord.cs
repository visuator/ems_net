using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("student_records", Schema = Schemas.Main)]
public abstract class StudentRecord : EntityBase, ISingleKeyEntity
{
    [Column("student_id")] public Guid StudentId { get; set; }

    public Student Student { get; set; }

    [Column("class_id")] public Guid ClassId { get; set; }

    public Class Class { get; set; }

    [Column("status")] public StudentRecordStatus Status { get; set; }
    [Column("type")] public StudentRecordType Type { get; set; }
    [Column("session_id")] public Guid? SessionId { get; set; }
    public StudentRecordSession? Session { get; set; }
    [Column("id")] public Guid Id { get; set; }
}