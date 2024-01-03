using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities.Abstractions;

public class StudentRecord : EntityBase, ISingleKeyEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = default!;
    public StudentRecordStatus Status { get; set; }
    public Guid? StudentRecordSessionId { get; set; }
    public StudentRecordSession? StudentRecordSession { get; set; }
    public Guid Id { get; set; }
}