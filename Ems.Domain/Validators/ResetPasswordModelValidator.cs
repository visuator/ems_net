using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
{
    public ResetPasswordModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) => await accountService.Exists(model.Email, token))
            .WithMessage(ErrorMessages.Account.IsNotExists)
            .WithName(nameof(ResetPasswordModel));
    }
}