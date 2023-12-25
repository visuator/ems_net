using AutoMapper;
using AutoMapper.AspNet.OData;
using EFCoreSecondLevelCacheInterceptor;
using Ems.Core.Entities;
using Ems.Infrastructure.Storage;
using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class LessonService : ILessonService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public LessonService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Import(List<ExcelLessonModel> models, CancellationToken token = new())
    {
        foreach (var lesson in models)
        {
            var existsLesson = await _dbContext.Lessons.NotCacheable().Where(x => x.Name == lesson.Name)
                .SingleOrDefaultAsync(token);
            if (existsLesson is null)
                await _dbContext.Lessons.AddAsync(_mapper.Map<Lesson>(lesson), token);
            else _mapper.Map(lesson, existsLesson);
        }

        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<List<LessonDto>> GetAll(ODataQueryOptions<LessonDto> query, CancellationToken token = new())
    {
        return await _dbContext.Lessons.GetQuery(_mapper, query).ToListAsync(token);
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Lessons.Where(x => x.Id == id).AnyAsync(token);
    }
}