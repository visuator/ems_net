using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;
using System.Text.Json.Serialization;

namespace Ems.Domain.Models;

public class GetGroupCurrentModel : IRequestTimeStamp
{
    [JsonIgnore] public Guid GroupId { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<GetGroupCurrentModel>
    {
        public Validator(IGroupService groupService)
        {
            RuleFor(x => x)
                .MustAsync(async (model, token) => await groupService.Exists(model.GroupId, token))
                //.WithMessage(ErrorMessages.Group.IsNotExists)
                .WithName(nameof(GetGroupInfoModel));
        }
    }
}