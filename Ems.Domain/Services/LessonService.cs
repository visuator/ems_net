using AutoMapper;
using Ems.Core.Entities;
using Ems.Infrastructure.Storages;
using Ems.Models.Excel;
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
            var existsLesson = await _dbContext.Lessons.Where(x => x.Name == lesson.Name).SingleOrDefaultAsync(token);
            if (existsLesson is null)
                await _dbContext.Lessons.AddAsync(_mapper.Map<Lesson>(lesson), token);
            else _mapper.Map(lesson, existsLesson);
        }

        await _dbContext.SaveChangesAsync(token);
    }
}