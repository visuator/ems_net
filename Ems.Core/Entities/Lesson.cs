using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class Lesson : EntityBase, ISingleKeyEntity
{
    public string Name { get; set; } = default!;
    public ICollection<Class> Classes { get; set; } = default!;
    public Guid Id { get; set; }
}