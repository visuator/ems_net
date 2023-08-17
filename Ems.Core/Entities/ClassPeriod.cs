using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

[Table("class_periods", Schema = Schemas.Main)]
public class ClassPeriod : EntityBase, ISingleKeyEntity
{
    [Column("starting_at")] public TimeSpan StartingAt { get; set; }

    [Column("ending_at")] public TimeSpan EndingAt { get; set; }

    [Column("name")] public string Name { get; set; }

    public ICollection<Class> Classes { get; set; }

    [Column("id")] public Guid Id { get; set; }
}