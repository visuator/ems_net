using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class PasswordResetEmail : Email
{
    [Column("password_reset_token")] public string PasswordResetToken { get; set; }

    [Column("password_reset_expires_at")] public DateTime PasswordResetExpiresAt { get; set; }
}