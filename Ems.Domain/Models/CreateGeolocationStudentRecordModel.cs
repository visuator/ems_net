using Ems.Models;
using FluentValidation;
using System.Text.Json.Serialization;

namespace Ems.Domain.Models;

public class CreateGeolocationStudentRecordModel : IAuthenticated
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public Guid ClassId { get; set; }
    public Guid SessionId { get; set; }

    [JsonIgnore] public Guid AccountId { get; set; }

    public class Validator : AbstractValidator<CreateGeolocationStudentRecordModel>
    {
        // public Validator(IClassService classService)
        // {
        //     RuleFor(x => x)
        //         .MustAsync(async (model, token) => await accountService.Exists(model.AccountId, token))
        //         .WithMessage(ErrorMessages.Account.IsNotExists)
        //         .MustAsync(async (model, token) => await classService.Exists(model.ClassId, token))
        //         .WithMessage(ErrorMessages.Class.IsNotExists)
        //         .WithName(nameof(CreateGeolocationStudentRecordModel));
        // }
    }
}