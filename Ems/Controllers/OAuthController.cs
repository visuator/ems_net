using AutoMapper;
using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Models;
using Ems.Services;
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
    private readonly ValidatorResolverService<AddExternalAccountModel> _addExternalAccountModelValidator;
    private readonly IExternalAccountService _externalAccountService;
    private readonly IMapper _mapper;
    private readonly ValidatorResolverService<OAuthLoginModel> _oauthLoginModelValidator;

    public OAuthController(IAccountService accountService, IMapper mapper,
        ValidatorResolverService<AddExternalAccountModel> addExternalAccountModelValidator,
        ValidatorResolverService<OAuthLoginModel> oauthLoginModelValidator, IExternalAccountService externalAccountService)
    {
        _accountService = accountService;
        _mapper = mapper;
        _addExternalAccountModelValidator = addExternalAccountModelValidator;
        _oauthLoginModelValidator = oauthLoginModelValidator;
        _externalAccountService = externalAccountService;
    }

    [HttpPost("google")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<GoogleOAuthModel>))]
    public async Task<IActionResult> HandleGoogle([FromBody] GoogleOAuthModel googleOAuthModel,
        CancellationToken token = new())
    {
        var model = _mapper.Map<OAuthLoginModel>(googleOAuthModel);
        return await _oauthLoginModelValidator.ForModel(model).HasModelStateFallback(ModelState)
            .OnSuccess(async (innerToken, innerModel) => await _accountService.Login(innerModel, innerToken))
            .Execute(token);
    }

    [HttpPost("google/link")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<GoogleOAuthModel>))]
    [TypeFilter(typeof(AuthenticatedActionFilter))]
    public async Task<IActionResult> HandleGoogleLink([FromBody] GoogleOAuthModel googleOAuthModel,
        CancellationToken token = new())
    {
        var model = _mapper.Map<AddExternalAccountModel>(googleOAuthModel);
        return await _addExternalAccountModelValidator.ForModel(model).HasModelStateFallback(ModelState)
            .OnSuccess(async (innerToken, innerModel) =>
                await _externalAccountService.AddExternalAccount(innerModel, innerToken))
            .Execute(token);
    }
}