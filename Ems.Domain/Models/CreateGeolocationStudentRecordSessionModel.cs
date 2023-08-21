using Ems.Core.Entities;
using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class CreateGeolocationStudentRecordSessionModel : IRequestTimeStamp, IAuthenticated, ICurrentClassBinding
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public DateTime RequestedAt { get; set; }
    public Guid AccountId { get; set; }
    public Class? CurrentClass { get; set; }

    public class Validator : AbstractValidator<CreateGeolocationStudentRecordSessionModel>
    {
        public Validator(IClassService classService)
        {
            RuleFor(x => x)
                .ChildRules(a => a.RuleFor(x => x.CurrentClass).NotNull())
                .WithMessage(ErrorMessages.Class.IsNotExists)
                .MustAsync(async (model, token) => await classService.Exists(model.CurrentClass!.Id, token))
                .WithMessage(ErrorMessages.Class.IsNotExists)
                .WithName(nameof(CreateGeolocationStudentRecordSessionModel));
        }
    }
}