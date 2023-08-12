using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

[Table("refresh_tokens", Schema = Schemas.Main)]
public class RefreshToken : EntityBase, ISingleKeyEntity
{
    [Column("account_id")] public Guid AccountId { get; set; }

    public Account Account { get; set; }

    [Column("session_token_id")] public Guid? SessionTokenId { get; set; }

    public RefreshToken? SessionToken { get; set; }

    [Column("value")] public string Value { get; set; }

    [Column("revoked_at")] public DateTime? RevokedAt { get; set; }

    [Column("used_at")] public DateTime? UsedAt { get; set; }

    [Column("id")] public Guid Id { get; set; }
}