using Ems.Domain.Constants;
using Ems.Domain.Services;
using FluentValidation;

namespace Ems.Models;

public class UpdateQrCodeStudentRecordStatusModel : IAuthenticated, IRequestTimeStamp
{
    public string Content { get; set; }
    public Guid AccountId { get; set; }
    public DateTime RequestedAt { get; set; }

    public class Validator : AbstractValidator<UpdateQrCodeStudentRecordStatusModel>
    {
        public Validator(IAccountService accountService, IStudentRecordSessionService studentRecordSessionService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (model, token) => await accountService.Exists(model.AccountId, token))
                .WithMessage(ErrorMessages.Account.IsNotExists)
                .MustAsync(async (model, token) =>
                    Guid.TryParse(model.Content.Split('-')[0], out var qrCodeStudentRecordSessionId) &&
                    await studentRecordSessionService.Exists(qrCodeStudentRecordSessionId, token))
                .WithMessage(ErrorMessages.StudentRecordSession.IsNotExists);
        }
    }
}