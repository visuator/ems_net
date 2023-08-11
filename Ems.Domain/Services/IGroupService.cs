using Ems.Models;
using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface IGroupService
{
    Task Import(List<ExcelGroupModel> models, CancellationToken token = new());
    Task<List<GroupDto>> GetAll(ODataQueryOptions<GroupDto> query, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
    Task<CurrentGroupInfoModel> GetGroupInfo(GetGroupInfoModel model, CancellationToken token = new());
}