using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class ClassVersion : EntityBase, ISingleKeyEntity
{
    public ClassVersionStatus Status { get; set; }
    public string Name { get; set; }

    public ICollection<Class> Classes { get; set; }
    public Guid Id { get; set; }
}