using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class IdlePeriod : EntityBase, ISingleKeyEntity
{
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    public required DateTime StartingAt { get; set; }
    public required DateTime EndingAt { get; set; }
    public required Guid Id { get; set; }
}