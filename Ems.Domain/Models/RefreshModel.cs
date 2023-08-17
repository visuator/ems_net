using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class RefreshModel : IRequestTimeStamp
{
    public string RefreshToken { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<RefreshModel>
    {
        public Validator(IAccountService accountService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (x, token) => !await accountService.IsRevoked(x.RefreshToken, token))
                .WithMessage(ErrorMessages.Session.IsRevoked)
                .WithName(nameof(RefreshModel));
        }
    }
}