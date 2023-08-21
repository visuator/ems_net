using Ems.Domain.Models;
using Ems.Domain.Services;
using Ems.Models;
using Ems.Services;
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
    private readonly ValidatorResolverService<CreateReplacementModel> _createReplacementModelValidator;
    private readonly ValidatorResolverService<GetGroupCurrentModel> _getGroupCurrentModelValidator;

    public ClassController(IClassService classService,
        ValidatorResolverService<CreateReplacementModel> createReplacementModelValidator,
        ValidatorResolverService<GetGroupCurrentModel> getGroupCurrentModelValidator)
    {
        _classService = classService;
        _createReplacementModelValidator = createReplacementModelValidator;
        _getGroupCurrentModelValidator = getGroupCurrentModelValidator;
    }

    [HttpPost("{id:guid}/replace")]
    public async Task<IActionResult> CreateReplacement([FromRoute] Guid id, [FromBody] CreateReplacementModel model,
        CancellationToken token = new())
    {
        model.SourceClassId = id;
        return await _createReplacementModelValidator.ForModel(model).HasModelStateFallback(ModelState)
            .OnSuccess(async (innerToken, innerModel) => await _classService.CreateReplacement(innerModel, innerToken))
            .Execute(token);
    }

    [HttpGet("group/{groupId:guid}")]
    public async Task<IActionResult> GetGroupInfo([FromRoute] Guid groupId, [FromQuery] DateTime requestedAt,
        CancellationToken token = new())
    {
        var model = new GetGroupCurrentModel { GroupId = groupId, RequestedAt = requestedAt };
        return await _getGroupCurrentModelValidator.ForModel(model).HasModelStateFallback(ModelState)
            .OnSuccess(async (innerToken, innerModel) => await _classService.GetGroupCurrent(innerModel, innerToken))
            .Execute<List<GroupClassInfoModel>>(token);
    }
}