using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class IdlePeriod : EntityBase, ISingleKeyEntity
{
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    public DateTime StartingAt { get; set; }
    public DateTime EndingAt { get; set; }
    public Guid Id { get; set; }
}