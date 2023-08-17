using Ems.Models;

namespace Ems.Domain.Services;

public interface IStudentRecordService
{
    Task Create(CreateStudentRecordModel model, CancellationToken token = new());
    Task Create(StudentRecordAsGeolocationModel model, CancellationToken token = new());
}