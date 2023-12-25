namespace Ems.Core.Entities.Abstractions;

public abstract class Person : EntityBase, ISingleKeyEntity
{
    public Guid AccountId { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? MiddleName { get; set; }

    public Guid Id { get; set; }
}