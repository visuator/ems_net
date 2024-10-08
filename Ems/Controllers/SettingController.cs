﻿using Asp.Versioning;
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
[Route("api/v{version:apiVersion}/settings")]
public class SettingController : ControllerBase
{
    private readonly ISettingService _settingService;

    public SettingController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ServiceFilter(typeof(ValidationActionFilter<CreateSettingModel>))]
    public async Task<IActionResult> Create([FromBody] CreateSettingModel model, CancellationToken token = new())
    {
        await _settingService.Create(model, token);
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll(ODataQueryOptions<SettingDto> query, CancellationToken token = new())
    {
        return Ok(await _settingService.GetAll(query, token));
    }

    [Authorize(Roles = "admin")]
    [HttpGet("qrCodeStudentRecordSessionOptions")]
    public async Task<IActionResult> GetQrCodeStudentRecordSessionOptions(CancellationToken token = new())
    {
        return Ok(await _settingService.GetQrCodeStudentRecordSessionOptions(token));
    }
}