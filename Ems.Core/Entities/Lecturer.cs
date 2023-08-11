using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

[Table("lecturers", Schema = Schemas.Main)]
public class Lecturer : Person
{
}