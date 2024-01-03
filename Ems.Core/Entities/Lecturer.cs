using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class Lecturer : Person
{
    public required ICollection<Class> Classes { get; set; }
}