using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class Setting : EntityBase, ISingleKeyEntity
{
    public required Quarter CurrentQuarter { get; set; }
    public required Guid Id { get; set; }
}