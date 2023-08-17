﻿using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    [ServiceFilter(typeof(ValidationActionFilter<CreateStudentRecordModel>))]
    public async Task<IActionResult> Create([FromBody] CreateStudentRecordModel model, CancellationToken token = new())
    {
        await _studentRecordService.Create(model, token);
        return Ok();
    }

    [HttpPost("geo")]
    [ServiceFilter(typeof(ValidationActionFilter<StudentRecordAsGeolocationModel>))]
    public async Task<IActionResult> CreateAsGeolocation([FromBody] StudentRecordAsGeolocationModel model,
        CancellationToken token = new())
    {
        await _studentRecordService.Create(model, token);
        return Ok();
    }

    [HttpPost("qr")]
    public async Task<IActionResult> CreateAsQrCode()
    {
        return Ok();
    }
}