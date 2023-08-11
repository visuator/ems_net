namespace Ems.Models;

public class PublishClassVersionModel
{
    public Guid ClassVersionId { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
}