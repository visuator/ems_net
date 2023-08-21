using Ems.Core.Entities.Enums;
using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Domain.Models;

public class AddExternalAccountModel
{
    public ExternalAccountProvider ExternalAccountProvider { get; set; }
    public string ExternalEmail { get; set; }
    public Guid AccountId { get; set; }

    public class Validator : AbstractValidator<AddExternalAccountModel>
    {
        public Validator(IAccountService accountService)
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
}