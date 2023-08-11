using Ems.Domain.Enums;
using Ems.Domain.Services.Import;
using Ems.Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/import")]
public class ImportController : ControllerBase
{
    private readonly ImportServiceProvider _importServiceProvider;

    public ImportController(ImportServiceProvider importServiceProvider)
    {
        _importServiceProvider = importServiceProvider;
    }

    [HttpPost("classPeriods")]
    public async Task<IActionResult> ImportClassPeriods(IFormFile file)
    {
        var service = _importServiceProvider.Get(ImportCategory.ClassPeriods, ImportFormat.Excel);
        var stream = file.OpenReadStream();
        await service.Import(stream);
        stream.Close();
        return Ok();
    }

    [HttpPost("classrooms")]
    public async Task<IActionResult> ImportClassrooms(IFormFile file)
    {
        var service = _importServiceProvider.Get(ImportCategory.Classrooms, ImportFormat.Excel);
        var stream = file.OpenReadStream();
        await service.Import(stream);
        stream.Close();
        return Ok();
    }

    [HttpPost("classVersion")]
    public async Task<IActionResult> ImportClassVersion(IFormFile file)
    {
        var service = _importServiceProvider.Get(ImportCategory.ClassVersion, ImportFormat.Excel);
        var stream = file.OpenReadStream();
        await service.Import(stream);
        stream.Close();
        return Ok();
    }

    [HttpPost("groups")]
    public async Task<IActionResult> ImportGroups(IFormFile file)
    {
        var service = _importServiceProvider.Get(ImportCategory.Groups, ImportFormat.Excel);
        var stream = file.OpenReadStream();
        await service.Import(stream);
        stream.Close();
        return Ok();
    }

    [HttpPost("lecturers")]
    public async Task<IActionResult> ImportLecturers(IFormFile file)
    {
        var service = _importServiceProvider.Get(ImportCategory.Lecturers, ImportFormat.Excel);
        var stream = file.OpenReadStream();
        await service.Import(stream);
        stream.Close();
        return Ok();
    }

    [HttpPost("lessons")]
    public async Task<IActionResult> ImportLessons(IFormFile file)
    {
        var service = _importServiceProvider.Get(ImportCategory.Lessons, ImportFormat.Excel);
        var stream = file.OpenReadStream();
        await service.Import(stream);
        stream.Close();
        return Ok();
    }

    [HttpPost("students")]
    public async Task<IActionResult> ImportStudents(IFormFile file)
    {
        var service = _importServiceProvider.Get(ImportCategory.Students, ImportFormat.Excel);
        var stream = file.OpenReadStream();
        await service.Import(stream);
        stream.Close();
        return Ok();
    }
}