using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (x, token) => await accountService.Exists(x.Email, token))
            .WithMessage(ErrorMessages.Account.IsNotExists)
            .MustAsync(async (x, token) => await accountService.IsConfirmed(x.Email, token))
            .WithMessage(ErrorMessages.Account.IsNotConfirmed)
            .MustAsync(async (x, token) => !await accountService.IsLocked(x.Email, x.RequestedAt, token))
            .WithMessage(ErrorMessages.Account.IsLocked)
            .MustAsync(
                async (x, token) => await accountService.CheckPassword(x.Email, x.Password, x.RequestedAt, token))
            .WithMessage(ErrorMessages.Account.InvalidPassword)
            .WithName(nameof(LoginModel));
    }
}