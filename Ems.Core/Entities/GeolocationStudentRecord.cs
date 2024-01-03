using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class GeolocationStudentRecord : StudentRecord
{
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
}