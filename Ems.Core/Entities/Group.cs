using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class Group : EntityBase, ISingleKeyEntity
{
    public Course Course { get; set; }

    public string Name { get; set; }

    public ICollection<Student> Students { get; set; }
    public ICollection<Class> Classes { get; set; }
    public Guid Id { get; set; }
}