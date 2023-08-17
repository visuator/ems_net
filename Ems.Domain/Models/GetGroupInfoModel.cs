using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class GetGroupInfoModel : IRequestTimeStamp
{
    public Guid Id { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<GetGroupInfoModel>
    {
        public Validator(IGroupService groupService)
        {
            RuleFor(x => x)
                .MustAsync(async (model, token) => await groupService.Exists(model.Id, token))
                .WithMessage(ErrorMessages.Group.IsNotExists)
                .WithName(nameof(GetGroupInfoModel));
        }
    }
}