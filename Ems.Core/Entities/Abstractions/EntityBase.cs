namespace Ems.Core.Entities.Abstractions;

public class EntityBase
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}