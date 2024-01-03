namespace Ems.Core.Entities.Abstractions;

public class Person : EntityBase, ISingleKeyEntity
{
    public required Guid AccountId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public required Guid Id { get; set; }
}