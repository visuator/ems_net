﻿using System.ComponentModel.DataAnnotations;
using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class LoginModel : IRequestTimeStamp
{
    [EmailAddress] public string Email { get; set; }

    public string Password { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<LoginModel>
    {
        public Validator(IAccountService accountService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (x, token) => await accountService.Exists(x.Email, token))
                .WithMessage(ErrorMessages.Account.IsNotExists)
                .MustAsync(async (x, token) => await accountService.IsConfirmed(x.Email, token))
                .WithMessage(ErrorMessages.Account.IsNotConfirmed)
                .MustAsync(async (x, token) => !await accountService.IsLocked(x.Email, x.RequestedAt, token))
                .WithMessage(ErrorMessages.Account.IsLocked)
                .MustAsync(
                    async (x, token) => await accountService.CheckPassword(x.Email, x.Password, x.RequestedAt, token))
                .WithMessage(ErrorMessages.Account.InvalidPassword)
                .WithName(nameof(LoginModel));
        }
    }
}