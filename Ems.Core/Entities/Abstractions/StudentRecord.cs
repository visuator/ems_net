using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities.Abstractions;

public class StudentRecord : EntityBase, ISingleKeyEntity
{
    public required Guid StudentId { get; set; }
    public required Student Student { get; set; }
    public required Guid ClassId { get; set; }
    public required Class Class { get; set; }
    public required StudentRecordStatus Status { get; set; }
    public Guid? StudentRecordSessionId { get; set; }
    public StudentRecordSession? StudentRecordSession { get; set; }
    public required Guid Id { get; set; }
}