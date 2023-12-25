using Ems.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/import")]
public class ImportController : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Import([FromForm] IFormFile file, [FromForm] ImportCategory category, CancellationToken token = new())
    {
        return Ok();
    }
}