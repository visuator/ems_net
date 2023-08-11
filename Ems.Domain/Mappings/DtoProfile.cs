using AutoMapper;
using Ems.Core.Entities;
using Ems.Models.Dtos;

namespace Ems.Domain.Mappings;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateProjection<ClassVersion, ClassVersionDto>()
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateProjection<Class, ClassDto>()
            .ForMember(x => x.Classroom, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Group, opt => opt.ExplicitExpansion())
            .ForMember(x => x.ClassVersion, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Lecturer, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Lesson, opt => opt.ExplicitExpansion())
            .ForMember(x => x.ClassPeriod, opt => opt.ExplicitExpansion())
            .ForMember(x => x.Template, opt => opt.ExplicitExpansion());
        CreateProjection<Classroom, ClassroomDto>();
        CreateProjection<Group, GroupDto>()
            .ForMember(x => x.Students, opt => opt.ExplicitExpansion());
        CreateProjection<Lecturer, LecturerDto>()
            .ForMember(x => x.Account, opt => opt.ExplicitExpansion());
        CreateProjection<Lesson, LessonDto>();
        CreateProjection<ClassPeriod, ClassPeriodDto>();
        CreateProjection<Student, StudentDto>()
            .ForMember(x => x.Account, opt => opt.ExplicitExpansion());
        CreateProjection<Account, AccountDto>()
            .ForMember(x => x.Roles, opt => opt.ExplicitExpansion())
            .ForMember(x => x.ExternalAccounts, opt => opt.ExplicitExpansion())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.PasswordSalt, opt => opt.Ignore())
            .ForMember(x => x.RefreshTokens, opt => opt.Ignore());
        CreateProjection<AccountRole, AccountRoleDto>()
            .ForMember(x => x.Account, opt => opt.ExplicitExpansion());
        CreateProjection<IdlePeriod, IdlePeriodDto>()
            .ForMember(x => x.Group, opt => opt.ExplicitExpansion());
        CreateProjection<ExternalAccount, ExternalAccountDto>()
            .ForMember(x => x.Account, opt => opt.ExplicitExpansion());
        CreateProjection<Setting, SettingDto>();
    }
}