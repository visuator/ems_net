using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class GeolocationStudentRecordSession : StudentRecordSession
{
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
}