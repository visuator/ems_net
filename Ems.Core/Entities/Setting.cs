using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("settings", Schema = Schemas.Main)]
public class Setting : EntityBase, ISingleKeyEntity
{
    [Column("current_quarter")] public Quarter CurrentQuarter { get; set; }

    [Column("id")] public Guid Id { get; set; }
}