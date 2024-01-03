using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class Group : EntityBase, ISingleKeyEntity
{
    public Course Course { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<Student> Students { get; set; } = default!;
    public ICollection<Class> Classes { get; set; } = default!;
    public Guid Id { get; set; }
}