using AutoMapper;
using Ems.Domain.Models;

namespace Ems.Domain.Mappings;

public class OAuthProfile : Profile
{
    public OAuthProfile()
    {
        CreateMap<GoogleOAuthModel, OAuthLoginModel>()
            .ConvertUsing<GoogleOAuthModelTypeConverter>();
        CreateMap<GoogleOAuthModel, AddExternalAccountModel>()
            .ConvertUsing<GoogleOAuthModelTypeConverter>();
    }
}