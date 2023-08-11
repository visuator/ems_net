using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class RefreshModelValidator : AbstractValidator<RefreshModel>
{
    public RefreshModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (x, token) => !await accountService.IsRevoked(x.RefreshToken, token))
            .WithMessage(ErrorMessages.Session.IsRevoked)
            .WithName(nameof(RefreshModel));
    }
}