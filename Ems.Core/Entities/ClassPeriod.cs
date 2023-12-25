using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class ClassPeriod : EntityBase, ISingleKeyEntity
{
    public TimeSpan StartingAt { get; set; }

    public TimeSpan EndingAt { get; set; }

    public string Name { get; set; }

    public ICollection<Class> Classes { get; set; }
    public Guid Id { get; set; }
}