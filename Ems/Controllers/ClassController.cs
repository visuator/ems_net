using Asp.Versioning;
using Ems.Domain.Models;
using Ems.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/classes")]
public class ClassController : ControllerBase
{
    private readonly IClassService _classService;

    public ClassController(IClassService classService)
    {
        _classService = classService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id:guid}/replace")]
    public async Task<IActionResult> CreateReplacement([FromRoute] Guid id, [FromBody] CreateReplacementModel model,
        CancellationToken token = new())
    {
        model.SourceClassId = id;
        await _classService.CreateReplacement(model, token);
        return Ok();
    }

    [Authorize(Roles = "admin,student,lecturer")]
    [HttpGet("group/{groupId:guid}")]
    public async Task<IActionResult> GetGroupInfo([FromRoute] Guid groupId, [FromQuery] DateTime requestedAt,
        CancellationToken token = new())
    {
        var model = new GetGroupCurrentModel { GroupId = groupId, RequestedAt = requestedAt };
        return Ok(await _classService.GetGroupCurrent(model, token));
    }
}