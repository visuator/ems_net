using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class ResetPasswordModel : IRequestTimeStamp
{
    public string Email { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<ResetPasswordModel>
    {
        public Validator(IAccountService accountService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (model, token) => await accountService.Exists(model.Email, token))
                .WithMessage(ErrorMessages.Account.IsNotExists)
                .WithName(nameof(ResetPasswordModel));
        }
    }
}