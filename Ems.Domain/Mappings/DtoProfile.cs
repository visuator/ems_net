using AutoMapper;
using Ems.Core.Entities;
using Ems.Core.Entities.Abstractions;
using Ems.Models.Dtos;

namespace Ems.Domain.Mappings;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<StudentRecord, StudentRecordDto>().IncludeAllDerived();
        CreateMap<StudentRecordSession, StudentRecordSessionDto>().IncludeAllDerived();

        CreateMap<GeolocationStudentRecord, GeolocationStudentRecordDto>();
        CreateMap<GeolocationStudentRecordSession, GeolocationStudentRecordSessionDto>();
        CreateMap<QrCodeStudentRecord, QrCodeStudentRecordDto>();
        CreateMap<QrCodeStudentRecordSession, QrCodeStudentRecordSessionDto>();
        CreateMap<QrCodeAttempt, QrCodeAttemptDto>();

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
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateMap<Lesson, LessonDto>()
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateMap<ClassPeriod, ClassPeriodDto>()
            .ForMember(x => x.Classes, opt => opt.ExplicitExpansion());
        CreateMap<Student, StudentDto>();
        CreateMap<IdlePeriod, IdlePeriodDto>()
            .ForMember(x => x.Group, opt => opt.ExplicitExpansion());
        CreateMap<Setting, SettingDto>();
    }
}