using Ems.Domain.Models;
using Ems.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface IIdlePeriodService
{
    Task Create(CreateIdlePeriodModel model, CancellationToken token = new());
    Task<List<IdlePeriodDto>> GetAll(ODataQueryOptions<IdlePeriodDto> query, CancellationToken token = new());
}