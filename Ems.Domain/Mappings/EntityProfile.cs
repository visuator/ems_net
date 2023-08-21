using AutoMapper;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Domain.Jobs;
using Ems.Domain.Models;
using Ems.Models;
using Ems.Models.Excel;

namespace Ems.Domain.Mappings;

public class EntityProfile : Profile
{
    public EntityProfile()
    {
        CreateMap<UpdateQrCodeStudentRecordStatusModel, QrCodeStudentRecord>();
        CreateMap<GeolocationStudentRecordSession, GeolocationStudentRecordSessionJob>()
            .ForMember(x => x.GeolocationStudentRecordSessionId, opt => opt.MapFrom(x => x.Id));
        CreateMap<CreateQrCodeStudentRecordSessionModel, QrCodeStudentRecordSession>();

        CreateMap<GeolocationStudentRecordSession, GeolocationStudentRecordSessionJob>();
        CreateMap<CreateGeolocationStudentRecordSessionModel, GeolocationStudentRecordSession>();
        CreateMap<CreateGeolocationStudentRecordModel, GeolocationStudentRecord>()
            .ForMember(x => x.Status, opt => opt.MapFrom(x => StudentRecordStatus.Created));

        CreateMap<CreateReplacementModel, Class>();

        CreateMap<Group, CurrentGroupInfoModel>();
        CreateMap<Class, GroupClassInfoModel>();

        CreateMap<CreateSettingModel, Setting>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<AddExternalAccountModel, ExternalAccount>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<CreateIdlePeriodModel, IdlePeriod>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<ExcelClassPeriodModel, ClassPeriod>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ExcelClassroomModel, Classroom>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ExcelClassVersionModel, ClassVersion>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Status, opt => opt.MapFrom(x => ClassVersionStatus.Draft))
            .ForMember(x => x.Classes, opt => opt.MapFrom(x => new List<ExcelClassModel>()));
        CreateMap<ExcelClassModel, Class>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ExcelGroupModel, Group>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ExcelLecturerModel, Lecturer>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForPath(x => x.Account.Phone, opt => opt.MapFrom(x => x.Phone))
            .ForPath(x => x.Account.Email, opt => opt.MapFrom(x => x.Email));
        CreateMap<ExcelStudentModel, Student>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForPath(x => x.Account.Phone, opt => opt.MapFrom(x => x.Phone))
            .ForPath(x => x.Account.Email, opt => opt.MapFrom(x => x.Email));
        CreateMap<ExcelLessonModel, Lesson>()
            .ForMember(x => x.Id, opt => opt.Ignore());
    }
}