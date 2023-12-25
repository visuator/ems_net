using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities.Abstractions;

public abstract class StudentRecord : EntityBase, ISingleKeyEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; }
    public Guid ClassId { get; set; }
    public Class Class { get; set; }
    public StudentRecordStatus Status { get; set; }
    public Guid? StudentRecordSessionId { get; set; }
    public StudentRecordSession? StudentRecordSession { get; set; }
    public Guid Id { get; set; }
}