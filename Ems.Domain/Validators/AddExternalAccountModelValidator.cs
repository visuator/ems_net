using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class AddExternalAccountModelValidator : AbstractValidator<AddExternalAccountModel>
{
    public AddExternalAccountModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) => await accountService.Exists(model.AccountId, token))
            .WithMessage(ErrorMessages.Account.IsNotExists)
            .MustAsync(async (model, token) => await accountService.IsConfirmed(model.AccountId, token))
            .WithMessage(ErrorMessages.Account.IsNotConfirmed)
            .MustAsync(async (model, token) =>
                !await accountService.ExternalAccountExists(model.ExternalEmail, token))
            .WithMessage(ErrorMessages.Account.External.AlreadyExists)
            .WithName(nameof(AddExternalAccountModel));
    }
}