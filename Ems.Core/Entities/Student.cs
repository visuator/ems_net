using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class Student : Person
{
    public required Guid GroupId { get; set; }

    public required Group Group { get; set; }
}