using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class ConfirmModelValidator : AbstractValidator<ConfirmModel>
{
    public ConfirmModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) =>
                await accountService.AnyByConfirmationToken(model.ConfirmationToken, token))
            .WithMessage(ErrorMessages.Account.ConfirmationTokenIsNotExists)
            .MustAsync(async (model, token) =>
                await accountService.CheckConfirmationExpiration(model.ConfirmationToken, model.RequestedAt, token))
            .WithMessage(ErrorMessages.Account.ConfirmationExpired)
            .WithName(nameof(ConfirmModel));
    }
}