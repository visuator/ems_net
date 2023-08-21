using System.ComponentModel.DataAnnotations.Schema;

namespace Ems.Core.Entities;

public class GeolocationStudentRecord : StudentRecord
{
    [Column("latitude")] public double Latitude { get; set; }
    [Column("longitude")] public double Longitude { get; set; }
}