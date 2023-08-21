using Ems.Domain.Models;

namespace Ems.Domain.Services;

public interface IStudentRecordSessionService
{
    Task Create(CreateGeolocationStudentRecordSessionModel model, CancellationToken token = new());
    Task Create(CreateQrCodeStudentRecordSessionModel sessionModel, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
}