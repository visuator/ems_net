using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class ConfirmPasswordResetModelValidator : AbstractValidator<ConfirmPasswordResetModel>
{
    public ConfirmPasswordResetModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) =>
                await accountService.AnyByPasswordResetToken(model.PasswordResetToken, token))
            .WithMessage(ErrorMessages.Account.ConfirmationTokenIsNotExists)
            .MustAsync(async (model, token) =>
                await accountService.CheckPasswordResetExpiration(model.PasswordResetToken, model.RequestedAt, token))
            .WithMessage(ErrorMessages.Account.ConfirmationExpired)
            .WithName(nameof(ConfirmModel));
    }
}