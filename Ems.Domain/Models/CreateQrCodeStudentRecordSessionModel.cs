﻿using Ems.Core.Entities;
using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class CreateQrCodeStudentRecordSessionModel : IRequestTimeStamp, IAuthenticated, ICurrentClassBinding
{
    public Guid StudentId { get; set; }
    public Guid AccountId { get; set; }
    public Class? CurrentClass { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<CreateQrCodeStudentRecordSessionModel>
    {
        public Validator(IAccountService accountService, IStudentService studentService, IClassService classService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .ChildRules(a => a.RuleFor(x => x.CurrentClass).NotNull())
                .WithMessage(ErrorMessages.Class.IsNotExists)
                .MustAsync(async (model, token) => await classService.Exists(model.CurrentClass!.Id, token))
                .WithMessage(ErrorMessages.Class.IsNotExists)
                .MustAsync(async (model, token) => await accountService.Exists(model.AccountId, token))
                .WithMessage(ErrorMessages.Account.IsNotExists)
                .MustAsync(async (model, token) => await studentService.Exists(model.StudentId, token))
                .WithName(nameof(CreateQrCodeStudentRecordSessionModel));
        }
    }
}