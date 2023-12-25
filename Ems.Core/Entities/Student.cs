using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class Student : Person
{
    public Guid GroupId { get; set; }

    public Group Group { get; set; }
}