using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class PublishClassVersionModel : IRequestTimeStamp
{
    public Guid ClassVersionId { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<PublishClassVersionModel>
    {
        public Validator(ISettingService settingService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (model, token) => await settingService.AnyAsync(token))
                .WithMessage(ErrorMessages.ClassVersion.SettingNotExists)
                .WithName(nameof(PublishClassVersionModel));
        }
    }
}