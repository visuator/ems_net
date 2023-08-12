using Ems.Domain.Services;
using Ems.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/classes")]
public class ClassController : ControllerBase
{
    private readonly IClassService _classService;
    private readonly IValidator<CreateReplacementModel> _createReplacementModelValidator;

    public ClassController(IClassService classService,
        IValidator<CreateReplacementModel> createReplacementModelValidator)
    {
        _classService = classService;
        _createReplacementModelValidator = createReplacementModelValidator;
    }

    [HttpPost("{id:guid}/replace")]
    public async Task<IActionResult> CreateReplacement([FromRoute] Guid id, [FromBody] CreateReplacementModel model,
        CancellationToken token = new())
    {
        model.SourceClassId = id;
        var result = await _createReplacementModelValidator.ValidateAsync(model, token);
        if (result.IsValid)
        {
            await _classService.CreateReplacement(model, token);
            return Ok();
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        return BadRequest(new ValidationProblemDetails(ModelState));
    }
}