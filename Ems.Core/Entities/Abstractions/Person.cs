using System.ComponentModel.DataAnnotations.Schema;

namespace Ems.Core.Entities.Abstractions;

public abstract class Person : EntityBase, ISingleKeyEntity
{
    [Column("account_id")] public Guid AccountId { get; set; }

    public Account Account { get; set; }

    [Column("first_name")] public string FirstName { get; set; }

    [Column("last_name")] public string LastName { get; set; }

    [Column("middle_name")] public string? MiddleName { get; set; }

    [Column("id")] public Guid Id { get; set; }
}