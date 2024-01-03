using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class GeolocationStudentRecordSession : StudentRecordSession
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}