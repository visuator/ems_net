using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("student_record_sessions", Schema = Schemas.Main)]
public abstract class StudentRecordSession : ISingleKeyEntity
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("started_at")]
    public DateTime StartedAt { get; set; }
    [Column("ending_at")]
    public DateTime EndingAt { get; set; }
    [Column("class_id")]
    public Guid ClassId { get; set; }
    public Class Class { get; set; }
    [Column("type")]
    public StudentRecordSessionType Type { get; set; }
    [Column("lecturer_id")]
    public Guid LecturerId { get; set; }
    public Lecturer Lecturer { get; set; }
    
    public ICollection<StudentRecord> StudentRecords { get; set; }
}