using Ems.Domain.Models;
using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/idlePeriods")]
public class IdlePeriodController : ControllerBase
{
    private readonly IIdlePeriodService _idlePeriodService;

    public IdlePeriodController(IIdlePeriodService idlePeriodService)
    {
        _idlePeriodService = idlePeriodService;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationActionFilter<CreateIdlePeriodModel>))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateIdlePeriod([FromBody] CreateIdlePeriodModel model,
        CancellationToken token = new())
    {
        await _idlePeriodService.Create(model, token);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll(ODataQueryOptions<IdlePeriodDto> query, CancellationToken token = new())
    {
        return Ok(await _idlePeriodService.GetAll(query, token));
    }
}