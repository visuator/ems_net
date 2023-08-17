using FluentValidation;

namespace Ems.Models;

public class CreateStudentRecordModel : IAuthenticated, IRequestTimeStamp
{
    public Guid ClassId { get; set; }
    public Guid AccountId { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<CreateStudentRecordModel>
    {
    }
}