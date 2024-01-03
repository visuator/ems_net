using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class ClassPeriod : EntityBase, ISingleKeyEntity
{
    public required TimeSpan StartingAt { get; set; }
    public required TimeSpan EndingAt { get; set; }
    public required string Name { get; set; }
    public required ICollection<Class> Classes { get; set; }
    public required Guid Id { get; set; }
}