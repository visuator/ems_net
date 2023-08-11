using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class OAuthLoginModelValidator : AbstractValidator<OAuthLoginModel>
{
    public OAuthLoginModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (x, token) => await accountService.ExternalAccountExists(x.ExternalEmail, token))
            .WithMessage(ErrorMessages.Account.IsNotExists)
            .WithName(nameof(OAuthLoginModel));
    }
}