using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("external_accounts", Schema = Schemas.Main)]
public class ExternalAccount : EntityBase, ISingleKeyEntity
{
    [Column("account_id")] public Guid AccountId { get; set; }

    public Account Account { get; set; }

    [Column("external_account_provider")] public ExternalAccountProvider ExternalAccountProvider { get; set; }

    [Column("external_email")] public string ExternalEmail { get; set; }

    [Column("id")] public Guid Id { get; set; }
}