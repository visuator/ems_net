using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Validators;

public class ReconfirmModelValidator : AbstractValidator<ReconfirmModel>
{
    public ReconfirmModelValidator(IAccountService accountService)
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (model, token) => await accountService.Exists(model.Email, token))
            .WithMessage(ErrorMessages.Account.IsNotExists)
            .WithName(nameof(ReconfirmModel));
    }
}