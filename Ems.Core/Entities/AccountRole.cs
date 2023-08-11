using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("account_roles", Schema = Schemas.Main)]
public class AccountRole : EntityBase
{
    [Column("account_id")] public Guid AccountId { get; set; }

    public Account Account { get; set; }

    [Column("role")] public Role Role { get; set; }
}