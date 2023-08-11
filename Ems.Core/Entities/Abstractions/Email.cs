using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities.Abstractions;

public abstract class Email : EntityBase, ISingleKeyEntity
{
    [Column("recipient")] public string Recipient { get; set; }

    [Column("type")] public EmailType Type { get; set; }

    [Column("status")] public EmailStatus Status { get; set; }

    [Column("id")] public Guid Id { get; set; }
}