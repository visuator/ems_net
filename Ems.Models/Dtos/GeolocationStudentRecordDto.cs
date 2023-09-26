namespace Ems.Models.Dtos;

public class GeolocationStudentRecordDto : StudentRecordDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}