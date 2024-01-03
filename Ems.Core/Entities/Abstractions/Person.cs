namespace Ems.Core.Entities.Abstractions;

public class Person : EntityBase, ISingleKeyEntity
{
    public Guid AccountId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public Guid Id { get; set; }
}