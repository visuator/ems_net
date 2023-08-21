﻿using Ems.Domain.Constants;
using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;

namespace Ems.Domain.Models;

public class RevokeSessionModel : IRequestTimeStamp
{
    public Guid AccountId { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<RevokeSessionModel>
    {
        public Validator(IAccountService accountService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (model, token) => await accountService.Exists(model.AccountId, token))
                .WithMessage(ErrorMessages.Account.IsNotExists)
                .WithName(nameof(RevokeSessionModel));
        }
    }
}