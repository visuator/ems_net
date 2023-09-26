namespace Ems.Models.Dtos;

public class GeolocationStudentRecordSessionDto : StudentRecordSessionDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}