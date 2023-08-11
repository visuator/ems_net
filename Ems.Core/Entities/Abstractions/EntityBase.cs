using System.ComponentModel.DataAnnotations.Schema;

namespace Ems.Core.Entities.Abstractions;

public class EntityBase
{
    [Column("created_at")] public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")] public DateTimeOffset UpdatedAt { get; set; }
}