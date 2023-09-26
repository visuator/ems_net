using Ems.Domain.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface IStudentRecordService
{
    Task<List<StudentRecordDto>> GetAll(ODataQueryOptions<StudentRecordDto> query, CancellationToken token = default);
    Task Create(CreateGeolocationStudentRecordModel model, CancellationToken token = new());
    Task Update(UpdateQrCodeStudentRecordStatusModel model, CancellationToken token = new());
}