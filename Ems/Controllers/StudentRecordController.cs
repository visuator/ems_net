using Asp.Versioning;
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
[Route("api/v{version:apiVersion}/studentRecords")]
public class StudentRecordController : ControllerBase
{
    private readonly IStudentRecordService _studentRecordService;

    public StudentRecordController(IStudentRecordService studentRecordService)
    {
        _studentRecordService = studentRecordService;
    }

    [Authorize(Roles = "student")]
    [HttpPost("geo")]
    [ServiceFilter(typeof(ValidationActionFilter<CreateGeolocationStudentRecordModel>))]
    public async Task<IActionResult> Create([FromBody] CreateGeolocationStudentRecordModel model,
        CancellationToken token = new())
    {
        await _studentRecordService.Create(model, token);
        return Ok();
    }

    [Authorize(Roles = "student")]
    [HttpPost("qr")]
    [ServiceFilter(typeof(ValidationActionFilter<UpdateQrCodeStudentRecordStatusModel>))]
    public async Task<IActionResult> Update([FromBody] UpdateQrCodeStudentRecordStatusModel model,
        CancellationToken token = new())
    {
        await _studentRecordService.Update(model, token);
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet()]
    public async Task<IActionResult> GetAll(ODataQueryOptions<StudentRecordDto> query, CancellationToken token = new())
    {
        return Ok(await _studentRecordService.GetAll(query, token));
    }
}