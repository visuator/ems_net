using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class DeleteExternalAccountModelValidator : AbstractValidator<DeleteExternalAccountModel>
{
    public DeleteExternalAccountModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) =>
                await accountService.ExternalAccountExists(model.Id, token))
            .WithMessage(ErrorMessages.Account.External.IsNotExists)
            .WithName(nameof(DeleteExternalAccountModel));
    }
}