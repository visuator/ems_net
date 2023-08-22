using AutoMapper;
using AutoMapper.AspNet.OData;
using EFCoreSecondLevelCacheInterceptor;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Domain.Jobs;
using Ems.Domain.Models;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class ClassVersionService : IClassVersionService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IScheduleService<PublishClassVersionJob> _publishClassVersionScheduleService;

    public ClassVersionService(EmsDbContext dbContext,
        IScheduleService<PublishClassVersionJob> publishClassVersionScheduleService, IMapper mapper)
    {
        _dbContext = dbContext;
        _publishClassVersionScheduleService = publishClassVersionScheduleService;
        _mapper = mapper;
    }

    public async Task Import(ExcelClassVersionModel model, CancellationToken token = new())
    {
        var classVersion = _mapper.Map<ClassVersion>(model);
        foreach (var classModel in model.Classes)
        {
            var group = await _dbContext.Groups.Where(x => x.Name == classModel.GroupName).Select(x => new { x.Id })
                .SingleAsync(token);
            var classPeriod = await _dbContext.ClassPeriods.Where(x => x.Name == classModel.ClassPeriodName)
                .Select(x => new { x.Id }).SingleAsync(token);
            var lecturer = await _dbContext.Lecturers
                .Where(x => x.LastName + ' ' + x.FirstName + ' ' + x.MiddleName == classModel.LecturerFullName)
                .Select(x => new { x.Id }).SingleAsync(token);
            var lesson = await _dbContext.Lessons.Where(x => x.Name == classModel.LessonName).Select(x => new { x.Id })
                .SingleAsync(token);
            var classroom = await _dbContext.Classrooms.Where(x => x.Name == classModel.ClassroomName)
                .Select(x => new { x.Id }).SingleAsync(token);

            classVersion.Classes.Add(_mapper.Map<Class>(classModel, opt => opt.AfterMap((src, dst) =>
            {
                dst.GroupId = group.Id;
                dst.ClassPeriodId = classPeriod.Id;
                dst.LecturerId = lecturer.Id;
                dst.LessonId = lesson.Id;
                dst.ClassroomId = classroom.Id;
            })));
        }

        await _dbContext.ClassVersions.AddAsync(classVersion, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task Publish(PublishClassVersionModel model, CancellationToken token = new())
    {
        await _publishClassVersionScheduleService.ScheduleJob(_mapper.Map<PublishClassVersionJob>(model), token);

        var classVersion = await _dbContext.ClassVersions.NotCacheable().AsTracking().Where(x => x.Id == model.ClassVersionId)
            .SingleAsync(token);
        classVersion.Status = ClassVersionStatus.Published;
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<List<ClassVersionDto>> GetAll(ODataQueryOptions<ClassVersionDto> query,
        CancellationToken token = new())
    {
        return await _dbContext.ClassVersions.GetQuery(_mapper, query).ToListAsync(token);
    }
}