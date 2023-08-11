using AutoMapper;
using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/oauth")]
public class OAuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IValidator<AddExternalAccountModel> _addExternalAccountModelValidator;
    private readonly IExternalAccountService _externalAccountService;
    private readonly IMapper _mapper;
    private readonly IValidator<OAuthLoginModel> _oauthLoginModel;

    public OAuthController(IAccountService accountService, IMapper mapper,
        IValidator<AddExternalAccountModel> addExternalAccountModelValidator,
        IValidator<OAuthLoginModel> oauthLoginModel, IExternalAccountService externalAccountService)
    {
        _accountService = accountService;
        _mapper = mapper;
        _addExternalAccountModelValidator = addExternalAccountModelValidator;
        _oauthLoginModel = oauthLoginModel;
        _externalAccountService = externalAccountService;
    }

    [HttpPost("google")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<GoogleOAuthModel>))]
    public async Task<IActionResult> HandleGoogle([FromBody] GoogleOAuthModel googleOAuthModel,
        CancellationToken token = new())
    {
        var model = _mapper.Map<OAuthLoginModel>(googleOAuthModel);
        var result = await _oauthLoginModel.ValidateAsync(model, token);
        if (result.IsValid)
            return Ok(await _accountService.Login(model, token));
        foreach (var error in result.Errors) ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        return BadRequest(new ValidationProblemDetails(ModelState));
    }

    [HttpPost("google/link")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<GoogleOAuthModel>))]
    [TypeFilter(typeof(AccountIdActionFilter))]
    public async Task<IActionResult> HandleGoogleLink([FromBody] GoogleOAuthModel googleOAuthModel,
        CancellationToken token = new())
    {
        var model = _mapper.Map<AddExternalAccountModel>(googleOAuthModel);
        var result = await _addExternalAccountModelValidator.ValidateAsync(model, token);
        if (result.IsValid)
        {
            await _externalAccountService.AddExternalAccount(model, token);
            return Ok();
        }

        foreach (var error in result.Errors) ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        return BadRequest(new ValidationProblemDetails(ModelState));
    }
}