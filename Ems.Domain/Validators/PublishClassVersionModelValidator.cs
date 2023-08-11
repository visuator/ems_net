using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class PublishClassVersionModelValidator : AbstractValidator<PublishClassVersionModel>
{
    public PublishClassVersionModelValidator(ISettingService settingService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) => await settingService.AnyAsync(token))
            .WithMessage(ErrorMessages.ClassVersion.SettingNotExists)
            .WithName(nameof(PublishClassVersionModel));
    }
}