using Ems.Domain.Models;
using Ems.Domain.Services;
using Ems.Interceptors;
using Ems.Models;
using Ems.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ValidatorResolverService<DeleteExternalAccountModel> _deleteExternalAccountModelValidator;
    private readonly IExternalAccountService _externalAccountService;
    private readonly ValidatorResolverService<RevokeSessionModel> _revokeSessionModelValidator;

    public AuthController(IAccountService accountService,
        ValidatorResolverService<DeleteExternalAccountModel> deleteExternalAccountModelValidator,
        IExternalAccountService externalAccountService,
        ValidatorResolverService<RevokeSessionModel> revokeSessionModelValidator)
    {
        _accountService = accountService;
        _deleteExternalAccountModelValidator = deleteExternalAccountModelValidator;
        _externalAccountService = externalAccountService;
        _revokeSessionModelValidator = revokeSessionModelValidator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<LoginModel>))]
    public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken token = new())
    {
        return Ok(await _accountService.Login(model, token));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<RefreshModel>))]
    public async Task<IActionResult> Refresh([FromBody] RefreshModel model, CancellationToken token = new())
    {
        return Ok(await _accountService.Refresh(model, token));
    }

    [HttpPost("confirm")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<ConfirmModel>))]
    public async Task<IActionResult> Confirm([FromBody] ConfirmModel model, CancellationToken token = new())
    {
        await _accountService.Confirm(model, token);
        return Ok();
    }

    [HttpPost("reconfirm")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<ReconfirmModel>))]
    public async Task<IActionResult> Reconfirm([FromBody] ReconfirmModel model, CancellationToken token = new())
    {
        await _accountService.Reconfirm(model, token);
        return Ok();
    }

    [HttpPost("{accountId:guid}/revokeSession")]
    public async Task<IActionResult> RevokeSession([FromRoute] Guid accountId, CancellationToken token = new())
    {
        var model = new RevokeSessionModel { AccountId = accountId };
        return await _revokeSessionModelValidator.ForModel(model).HasModelStateFallback(ModelState)
            .OnSuccess(async (innerToken, innerModel) => await _accountService.RevokeSession(innerModel, innerToken))
            .Execute(token);
    }

    [HttpPost("resetPassword")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<ResetPasswordModel>))]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model, CancellationToken token = new())
    {
        await _accountService.ResetPassword(model, token);
        return Ok();
    }

    [HttpPost("confirmPasswordReset")]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationActionFilter<ConfirmPasswordResetModel>))]
    public async Task<IActionResult> ConfirmPasswordReset([FromBody] ConfirmPasswordResetModel model,
        CancellationToken token = new())
    {
        await _accountService.ConfirmPasswordReset(model, token);
        return Ok();
    }

    [HttpDelete("external/{id:guid}")]
    public async Task<IActionResult> DeleteExternalAccount([FromRoute] Guid id, CancellationToken token = new())
    {
        var model = new DeleteExternalAccountModel { Id = id };
        return await _deleteExternalAccountModelValidator.ForModel(model).HasModelStateFallback(ModelState)
            .OnSuccess(async (innerToken, innerModel) =>
                await _externalAccountService.DeleteExternalAccount(innerModel, innerToken))
            .Execute(token);
    }
}