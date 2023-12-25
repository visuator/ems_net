using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class Setting : EntityBase, ISingleKeyEntity
{
    public Quarter CurrentQuarter { get; set; }
    public Guid Id { get; set; }
}