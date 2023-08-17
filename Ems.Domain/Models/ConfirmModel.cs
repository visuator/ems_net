using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class ConfirmModel : IRequestTimeStamp
{
    public string ConfirmationToken { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<ConfirmModel>
    {
        public Validator(IAccountService accountService)
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
}