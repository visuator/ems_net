using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class CreateGeolocationStudentRecordSessionModel : IRequestTimeStamp
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public Guid ClassId { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<CreateGeolocationStudentRecordSessionModel>
    {
        public Validator(IClassService classService)
        {
            RuleFor(x => x)
                .MustAsync(async (model, token) => await classService.Exists(model.ClassId, token))
                .WithMessage(ErrorMessages.Class.IsNotExists)
                .WithName(nameof(CreateGeolocationStudentRecordSessionModel));
        }
    }
}