using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("class_versions", Schema = Schemas.Main)]
public class ClassVersion : EntityBase, ISingleKeyEntity
{
    [Column("id")] public Guid Id { get; set; }
    [Column("status")] public ClassVersionStatus Status { get; set; }

    [Column("name")] public string Name { get; set; }

    public ICollection<Class> Classes { get; set; }
}