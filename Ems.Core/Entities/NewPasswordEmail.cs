using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class NewPasswordEmail : Email
{
    [Column("password")] public string Password { get; set; }
}