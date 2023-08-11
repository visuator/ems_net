using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

[Table("students", Schema = Schemas.Main)]
public class Student : Person
{
    [Column("group_id")] public Guid GroupId { get; set; }

    public Group Group { get; set; }
}