using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class DeleteExternalAccountModel
{
    public Guid Id { get; set; }

    public class Validator : AbstractValidator<DeleteExternalAccountModel>
    {
        public Validator(IAccountService accountService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (model, token) =>
                    await accountService.ExternalAccountExists(model.Id, token))
                .WithMessage(ErrorMessages.Account.External.IsNotExists)
                .WithName(nameof(DeleteExternalAccountModel));
        }
    }
}