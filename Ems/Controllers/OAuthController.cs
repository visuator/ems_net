using AutoMapper;
using Ems.Domain.Models;
using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Services;
using Ems.Services.Validation;
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
    private readonly IExternalAccountService _externalAccountService;
    private readonly IMapper _mapper;
    private readonly IValidatorResolverService _validatorResolverService;

    public OAuthController(IAccountService accountService, IExternalAccountService externalAccountService, IMapper mapper, IValidatorResolverService validatorResolverService)
    {
        _accountService = accountService;
        _externalAccountService = externalAccountService;
        _mapper = mapper;
        _validatorResolverService = validatorResolverService;
    }

    [HttpPost("google")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<GoogleOAuthModel>))]
    public async Task<IActionResult> HandleGoogle([FromBody] GoogleOAuthModel googleOAuthModel,
        CancellationToken token = new())
    {
        var model = _mapper.Map<OAuthLoginModel>(googleOAuthModel);
        return await _validatorResolverService.ForModel(model)
            .WithModelState(ModelState)
            .WithAsyncCallback(async (m, t) => await _accountService.Login(m, t))
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
        return await _validatorResolverService.ForModel(model)
            .WithModelState(ModelState)
            .WithAsyncCallback(async (m, t) => await _externalAccountService.AddExternalAccount(m, t))
            .Execute(token);
    }
}