using Ems.Domain.Services;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Controllers;

// роль админа
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/lecturers")]
public class LecturerController : ControllerBase
{
    private readonly ILecturerService _lecturerService;

    public LecturerController(ILecturerService lecturerService)
    {
        _lecturerService = lecturerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(ODataQueryOptions<LecturerDto> query, CancellationToken token = new())
    {
        return Ok(await _lecturerService.GetAll(query, token));
    }
}