using FluentValidation;
using NetTopologySuite.Geometries;

namespace Ems.Models;

public class StudentRecordAsGeolocationModel : IAuthenticated
{
    public Point Location { set; get; }
    public Guid ClassId { get; set; }
    public Guid AccountId { get; set; }

    public class Validator : AbstractValidator<StudentRecordAsGeolocationModel>
    {
    }
}