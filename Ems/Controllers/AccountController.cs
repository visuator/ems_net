using Ems.Domain.Models;
using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/accounts")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IExternalAccountService _externalAccountService;

    public AccountController(IAccountService accountService, IExternalAccountService externalAccountService)
    {
        _accountService = accountService;
        _externalAccountService = externalAccountService;
    }

    [HttpGet("authenticated")]
    [TypeFilter(typeof(AuthenticatedActionFilter))]
    public async Task<IActionResult> GetAuthenticated([FromQuery] GetAuthenticatedModel model)
    {
        return Ok(await _accountService.GetAuthenticated(model));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(ODataQueryOptions<AccountDto> query, CancellationToken token = new())
    {
        return Ok(await _accountService.GetAll(query, token));
    }

    [HttpGet("external")]
    public async Task<IActionResult> GetAll(ODataQueryOptions<ExternalAccountDto> query,
        CancellationToken token = new())
    {
        return Ok(await _externalAccountService.GetAll(query, token));
    }
}