using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class CreateIdlePeriodModelValidator : AbstractValidator<CreateIdlePeriodModel>
{
    public CreateIdlePeriodModelValidator(IGroupService groupService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) => await groupService.Exists(model.GroupId!.Value, token))
            .When(x => x.GroupId is not null)
            .WithMessage(ErrorMessages.Group.IsNotExists)
            .WithName(nameof(CreateIdlePeriodModel));
    }
}