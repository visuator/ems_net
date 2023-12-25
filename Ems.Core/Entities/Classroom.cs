using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class Classroom : EntityBase, ISingleKeyEntity
{
    public string Name { get; set; }

    public ICollection<Class> Classes { get; set; }
    public Guid Id { get; set; }
}