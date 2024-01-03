namespace Ems.Core.Entities.Abstractions;

public class StudentRecordSession : ISingleKeyEntity
{
    public required DateTime StartedAt { get; set; }
    public required DateTime EndingAt { get; set; }
    public required Guid ClassId { get; set; }
    public required Class Class { get; set; }
    public required Guid LecturerId { get; set; }
    public required Lecturer Lecturer { get; set; }

    public required ICollection<StudentRecord> StudentRecords { get; set; }
    public required Guid Id { get; set; }
}