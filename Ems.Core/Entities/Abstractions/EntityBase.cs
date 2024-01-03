namespace Ems.Core.Entities.Abstractions;

public class EntityBase
{
    public required bool IsDeleted { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}