using AutoMapper;
using Ems.Core.Entities.Enums;
using Ems.Domain.Models;
using Ems.Infrastructure.Services;
using Ems.Models;

namespace Ems.Domain.Mappings;

public class GoogleOAuthModelTypeConverter : ITypeConverter<GoogleOAuthModel, OAuthLoginModel>,
    ITypeConverter<GoogleOAuthModel, AddExternalAccountModel>
{
    private readonly IJwtService _jwtService;

    public GoogleOAuthModelTypeConverter(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public AddExternalAccountModel Convert(GoogleOAuthModel source, AddExternalAccountModel destination,
        ResolutionContext context)
    {
        var decodedToken = _jwtService.DecodeToken(source.Credential);
        if (!decodedToken.Claims.Any(x => x.Type == "email_verified" && System.Convert.ToBoolean(x.Value)))
            return new AddExternalAccountModel();
        return new AddExternalAccountModel
        {
            ExternalAccountProvider = ExternalAccountProvider.Google,
            ExternalEmail = decodedToken.Claims.First(x => x.Type == "email").Value,
            AccountId = source.AccountId
        };
    }

    public OAuthLoginModel Convert(GoogleOAuthModel source, OAuthLoginModel destination, ResolutionContext context)
    {
        var decodedToken = _jwtService.DecodeToken(source.Credential);
        if (!decodedToken.Claims.Any(x => x.Type == "email_verified" && System.Convert.ToBoolean(x.Value)))
            return new OAuthLoginModel();
        return new OAuthLoginModel
        {
            ExternalEmail = decodedToken.Claims.First(x => x.Type == "email").Value
        };
    }
}