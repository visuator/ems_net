using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

[Table("lessons", Schema = Schemas.Main)]
public class Lesson : EntityBase, ISingleKeyEntity
{
    [Column("name")] public string Name { get; set; }

    [Column("id")] public Guid Id { get; set; }
    
    public ICollection<Class> Classes { get; set; }
}