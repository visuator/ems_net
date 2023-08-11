using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class GetGroupInfoModelValidator : AbstractValidator<GetGroupInfoModel>
{
    public GetGroupInfoModelValidator(IGroupService groupService)
    {
        RuleFor(x => x)
            .MustAsync(async (model, token) => await groupService.Exists(model.Id, token))
            .WithMessage(ErrorMessages.Group.IsNotExists)
            .WithName(nameof(GetGroupInfoModel));
    }
}