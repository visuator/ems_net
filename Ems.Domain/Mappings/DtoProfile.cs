using AutoMapper;
using Ems.Core.Entities;
using Ems.Models.Dtos;

namespace Ems.Domain.Mappings;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<Group, GroupDto>()
            .ForMember(x => x.Students, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateMap<ClassVersion, ClassVersionDto>()
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateMap<Class, ClassDto>()
            .ForMember(x => x.Classroom, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Group, opt => opt.ExplicitExpansion())
            .ForMember(x => x.ClassVersion, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Lecturer, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Lesson, opt => opt.ExplicitExpansion())
            .ForMember(x => x.ClassPeriod, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Template, opt => opt.ExplicitExpansion());
        CreateMap<Classroom, ClassroomDto>();
        CreateMap<Lecturer, LecturerDto>()
            .ForMember(x => x.Account, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateMap<Lesson, LessonDto>()
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateMap<ClassPeriod, ClassPeriodDto>()
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateMap<Student, StudentDto>()
            .ForMember(x => x.Account, opt => opt.ExplicitExpansion());
        CreateMap<Account, AccountDto>()
            .ForMember(x => x.Roles, opt => opt.ExplicitExpansion())
            .ForMember(x => x.ExternalAccounts, opt => opt.ExplicitExpansion())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.PasswordSalt, opt => opt.Ignore())
            .ForMember(x => x.RefreshTokens, opt => opt.Ignore());
        CreateMap<AccountRole, AccountRoleDto>()
            .ForMember(x => x.Account, opt => opt.ExplicitExpansion());
        CreateMap<IdlePeriod, IdlePeriodDto>()
            .ForMember(x => x.Group, opt => opt.ExplicitExpansion());
        CreateMap<ExternalAccount, ExternalAccountDto>()
            .ForMember(x => x.Account, opt => opt.ExplicitExpansion());
        CreateMap<Setting, SettingDto>();
    }
}