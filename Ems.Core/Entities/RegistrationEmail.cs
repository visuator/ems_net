using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class RegistrationEmail : Email
{
    [Column("password")] public string Password { get; set; }

    [Column("confirmation_token")] public string ConfirmationToken { get; set; }

    [Column("confirmation_expires_at")] public DateTime ConfirmationExpiresAt { get; set; }
}