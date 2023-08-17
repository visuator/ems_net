using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using NetTopologySuite.Geometries;

namespace Ems.Core.Entities;

[Table("geolocation_student_records", Schema = Schemas.Main)]
public class GeolocationStudentRecord : EntityBase
{
    [Column("student_id")] public Guid StudentId { get; set; }

    [Column("class_id")] public Guid ClassId { get; set; }

    public StudentRecord StudentRecord { get; set; }

    [Column("location")] public Point Location { get; set; }
}