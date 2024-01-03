using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class GeolocationStudentRecord : StudentRecord
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}