using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class RevokeSessionModelValidator : AbstractValidator<RevokeSessionModel>
{
    public RevokeSessionModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) => await accountService.Exists(model.AccountId, token))
            .WithMessage(ErrorMessages.Account.IsNotExists)
            .WithName(nameof(RevokeSessionModel));
    }
}