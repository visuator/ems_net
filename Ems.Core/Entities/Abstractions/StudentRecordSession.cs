namespace Ems.Core.Entities.Abstractions;

public class StudentRecordSession : ISingleKeyEntity
{
    public DateTime StartedAt { get; set; }
    public DateTime EndingAt { get; set; }
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = default!;
    public Guid LecturerId { get; set; }
    public Lecturer Lecturer { get; set; } = default!;

    public ICollection<StudentRecord> StudentRecords { get; set; } = default!;
    public Guid Id { get; set; }
}