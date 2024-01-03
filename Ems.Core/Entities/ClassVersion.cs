using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class ClassVersion : EntityBase, ISingleKeyEntity
{
    public required ClassVersionStatus Status { get; set; }
    public required string Name { get; set; }
    public required ICollection<Class> Classes { get; set; }
    public required Guid Id { get; set; }
}