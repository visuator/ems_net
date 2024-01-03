using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class Classroom : EntityBase, ISingleKeyEntity
{
    public required string Name { get; set; }
    public required ICollection<Class> Classes { get; set; }
    public required Guid Id { get; set; }
}