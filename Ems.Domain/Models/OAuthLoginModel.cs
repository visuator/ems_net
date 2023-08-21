using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Domain.Models;

public class OAuthLoginModel
{
    public string ExternalEmail { get; set; }

    public class Validator : AbstractValidator<OAuthLoginModel>
    {
        public Validator(IAccountService accountService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (x, token) => await accountService.ExternalAccountExists(x.ExternalEmail, token))
                .WithMessage(ErrorMessages.Account.IsNotExists)
                .WithName(nameof(OAuthLoginModel));
        }
    }
}