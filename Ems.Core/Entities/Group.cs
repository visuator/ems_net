using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class Group : EntityBase, ISingleKeyEntity
{
    public required Course Course { get; set; }
    public required string Name { get; set; }
    public required ICollection<Student> Students { get; set; }
    public required ICollection<Class> Classes { get; set; }
    public required Guid Id { get; set; }
}