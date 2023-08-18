using Ems.Models;

namespace Ems.Domain.Services;

public interface IStudentRecordService
{
    Task Create(CreateGeolocationStudentRecordModel model, CancellationToken token = new());
}