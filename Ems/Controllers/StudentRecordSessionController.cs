using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/studentRecordSessions")]
public class StudentRecordSessionController : ControllerBase
{
    private readonly IStudentRecordSessionService _studentRecordSessionService;

    public StudentRecordSessionController(IStudentRecordSessionService studentRecordSessionService)
    {
        _studentRecordSessionService = studentRecordSessionService;
    }

    [HttpPost("gps")]
    [ServiceFilter(typeof(ValidationActionFilter<CreateGeolocationStudentRecordSessionModel>))]
    public async Task<IActionResult> Create(CreateGeolocationStudentRecordSessionModel model,
        CancellationToken token = new())
    {
        await _studentRecordSessionService.Create(model, token);
        return Ok();
    }
    
    [HttpPost("qr")]
    [ServiceFilter(typeof(ValidationActionFilter<CreateQrCodeStudentRecordSessionModel>))]
    public async Task<IActionResult> Create(CreateQrCodeStudentRecordSessionModel sessionModel,
        CancellationToken token = new())
    {
        await _studentRecordSessionService.Create(sessionModel, token);
        return Ok();
    }
}