using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class Lecturer : Person
{
    public ICollection<Class> Classes { get; set; }
}