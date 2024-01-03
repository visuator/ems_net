using Asp.Versioning;
using Ems.Domain.Services;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/classrooms")]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll(ODataQueryOptions<ClassroomDto> query, CancellationToken token = new())
    {
        return Ok(await _classroomService.GetAll(query, token));
    }
}