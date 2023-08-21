using AutoMapper;
using Ems.Infrastructure.Options;
using Ems.Models;

namespace Ems.Domain.Mappings;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        CreateMap<QrCodeStudentRecordSessionOptions, QrCodeStudentRecordSessionOptionsModel>();
    }
}