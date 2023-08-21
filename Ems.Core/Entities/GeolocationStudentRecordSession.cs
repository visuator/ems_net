using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class GeolocationStudentRecordSession : StudentRecordSession
{
    [Column("latitude")] public double Latitude { get; set; }
    [Column("longitude")] public double Longitude { get; set; }
}