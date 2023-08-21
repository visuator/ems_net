using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Models;

public class ReconfirmModel : IRequestTimeStamp
{
    public string Email { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<ReconfirmModel>
    {
        public Validator(IAccountService accountService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (model, token) => await accountService.Exists(model.Email, token))
                .WithMessage(ErrorMessages.Account.IsNotExists)
                .WithName(nameof(ReconfirmModel));
        }
    }
}