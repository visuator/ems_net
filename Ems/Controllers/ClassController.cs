using Ems.Domain.Models;
using Ems.Domain.Services;
using Ems.Models;
using Ems.Services;
using Ems.Services.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

// роль админа
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/classes")]
public class ClassController : ControllerBase
{
    private readonly IClassService _classService;
    private readonly IValidatorResolverService _validatorResolverService;

    public ClassController(IClassService classService, IValidatorResolverService validatorResolverService)
    {
        _classService = classService;
        _validatorResolverService = validatorResolverService;
    }

    [HttpPost("{id:guid}/replace")]
    public async Task<IActionResult> CreateReplacement([FromRoute] Guid id, [FromBody] CreateReplacementModel model,
        CancellationToken token = new())
    {
        model.SourceClassId = id;
        return await _validatorResolverService.ForModel(model)
            .WithModelState(ModelState)
            .WithAsyncCallback(async (m, t) => await _classService.CreateReplacement(m, t))
            .Execute(token);
    }

    [HttpGet("group/{groupId:guid}")]
    public async Task<IActionResult> GetGroupInfo([FromRoute] Guid groupId, [FromQuery] DateTime requestedAt,
        CancellationToken token = new())
    {
        var model = new GetGroupCurrentModel { GroupId = groupId, RequestedAt = requestedAt };
        return await _validatorResolverService.ForModel(model)
            .WithModelState(ModelState)
            .WithAsyncCallback(async (m, t) => await _classService.GetGroupCurrent(m, t))
            .Execute(token);
    }
}