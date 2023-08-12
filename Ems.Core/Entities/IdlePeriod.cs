using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

[Table("idle_periods", Schema = Schemas.Main)]
public class IdlePeriod : EntityBase, ISingleKeyEntity
{
    [Column("group_id")] public Guid? GroupId { get; set; }

    public Group? Group { get; set; }

    [Column("starting_at")] public DateTime StartingAt { get; set; }

    [Column("ending_at")] public DateTime EndingAt { get; set; }

    [Column("id")] public Guid Id { get; set; }
}