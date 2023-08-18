using Ems.Core.Entities;
using Ems.Models;

namespace Ems.Domain.Services;

public interface IStudentRecordSessionService
{
    Task<StudentRecordSession> Get(Guid id, CancellationToken token = new());
    Task Create(CreateGeolocationStudentRecordSessionModel model, CancellationToken token = new());
}