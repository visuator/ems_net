using Ems.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface ISettingService
{
    Task<bool> AnyAsync(CancellationToken token = new());
    Task<List<SettingDto>> GetAll(ODataQueryOptions<SettingDto> query, CancellationToken token = new());
    Task Create(CreateSettingModel model, CancellationToken token = new());
    Task<QrCodeStudentRecordSessionOptionsModel> GetQrCodeStudentRecordSessionOptions(CancellationToken token = new());
}