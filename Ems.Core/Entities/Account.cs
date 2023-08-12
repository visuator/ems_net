using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

[Table("accounts", Schema = Schemas.Main)]
public class Account : EntityBase, ISingleKeyEntity
{
    [Column("email")] public string Email { get; set; }

    [Column("phone")] public string Phone { get; set; }

    [Column("password_salt")] public string PasswordSalt { get; set; }

    [Column("password_hash")] public string PasswordHash { get; set; }

    [Column("confirmed_at")] public DateTime? ConfirmedAt { get; set; }

    [Column("confirmation_expires_at")] public DateTime? ConfirmationExpiresAt { get; set; }

    [Column("confirmation_token")] public string? ConfirmationToken { get; set; }

    [Column("password_reset_expires_at")] public DateTime? PasswordResetExpiresAt { get; set; }

    [Column("password_reset_token")] public string? PasswordResetToken { get; set; }

    [Column("locked_at")] public DateTime? LockExpiresAt { get; set; }

    [Column("failed_attempts")] public int FailedAttempts { get; set; }

    public ICollection<AccountRole> Roles { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public ICollection<ExternalAccount> ExternalAccounts { get; set; }

    [Column("id")] public Guid Id { get; set; }
}