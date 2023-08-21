using Ems.Domain.Models;
using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Controllers;

// роль админа
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/classVersions")]
public class ClassVersionController : ControllerBase
{
    private readonly IClassVersionService _classVersionService;

    public ClassVersionController(IClassVersionService classVersionService)
    {
        _classVersionService = classVersionService;
    }

    [HttpPost("publish")]
    [ServiceFilter(typeof(ValidationActionFilter<PublishClassVersionModel>))]
    public async Task<IActionResult> Publish([FromBody] PublishClassVersionModel model, CancellationToken token = new())
    {
        await _classVersionService.Publish(model, token);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(ODataQueryOptions<ClassVersionDto> query)
    {
        return Ok(await _classVersionService.GetAll(query));
    }
}