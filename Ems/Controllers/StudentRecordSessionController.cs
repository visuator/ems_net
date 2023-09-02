using Ems.Domain.Models;
using Ems.Domain.Services;
using Ems.Interceptors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

// сделать роль только для преподавателя
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
    public async Task<IActionResult> Create([FromBody] CreateGeolocationStudentRecordSessionModel model,
        CancellationToken token = new())
    {
        await _studentRecordSessionService.Create(model, token);
        return Ok();
    }

    [HttpPost("qr")]
    [ServiceFilter(typeof(ValidationActionFilter<CreateQrCodeStudentRecordSessionModel>))]
    public async Task<IActionResult> Create([FromBody] CreateQrCodeStudentRecordSessionModel sessionModel,
        CancellationToken token = new())
    {
        await _studentRecordSessionService.Create(sessionModel, token);
        return Ok();
    }
}