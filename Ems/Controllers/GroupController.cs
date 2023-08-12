using Ems.Domain.Services;
using Ems.Models;
using Ems.Models.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IValidator<GetGroupInfoModel> _getGroupInfoModelValidator;

    public GroupController(IGroupService groupService, IValidator<GetGroupInfoModel> getGroupInfoModelValidator)
    {
        _groupService = groupService;
        _getGroupInfoModelValidator = getGroupInfoModelValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(ODataQueryOptions<GroupDto> query, CancellationToken token = new())
    {
        return Ok(await _groupService.GetAll(query, token));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetGroupInfo([FromRoute] Guid id, [FromQuery] DateTime requestedAt,
        CancellationToken token = new())
    {
        var model = new GetGroupInfoModel() { Id = id, RequestedAt = requestedAt };
        var result = await _getGroupInfoModelValidator.ValidateAsync(model, token);
        if (result.IsValid)
            return Ok(await _groupService.GetGroupInfo(model, token));
        foreach (var error in result.Errors)
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        return BadRequest(new ValidationProblemDetails(ModelState));
    }
}