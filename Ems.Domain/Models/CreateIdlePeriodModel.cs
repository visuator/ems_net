using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Domain.Models;

public class CreateIdlePeriodModel
{
    public Guid? GroupId { get; set; }
    public DateTime StartingAt { get; set; }
    public DateTime EndingAt { get; set; }

    public class Validator : AbstractValidator<CreateIdlePeriodModel>
    {
        public Validator(IGroupService groupService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (model, token) => await groupService.Exists(model.GroupId!.Value, token))
                .When(x => x.GroupId is not null)
                .WithMessage(ErrorMessages.Group.IsNotExists)
                .WithName(nameof(CreateIdlePeriodModel));
        }
    }
}