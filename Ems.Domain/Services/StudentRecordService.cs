using Ems.Models;

namespace Ems.Domain.Services;

public class StudentRecordService : IStudentRecordService
{
    public async Task Create(CreateStudentRecordModel model, CancellationToken token = new())
    {
        
    }

    public async Task Create(StudentRecordAsGeolocationModel model, CancellationToken token = new())
    {
    }
}