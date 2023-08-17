using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("groups", Schema = Schemas.Main)]
public class Group : EntityBase, ISingleKeyEntity
{
    [Column("course")] public Course Course { get; set; }

    [Column("name")] public string Name { get; set; }

    public ICollection<Student> Students { get; set; }
    public ICollection<Class> Classes { get; set; }

    [Column("id")] public Guid Id { get; set; }
}