using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class ClassVersion : EntityBase, ISingleKeyEntity
{
    public ClassVersionStatus Status { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<Class> Classes { get; set; } = default!;
    public Guid Id { get; set; }
}