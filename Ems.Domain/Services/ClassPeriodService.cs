using AutoMapper;
using Ems.Core.Entities;
using Ems.Infrastructure.Storages;
using Ems.Models.Excel;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class ClassPeriodService : IClassPeriodService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ClassPeriodService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Import(List<ExcelClassPeriodModel> models, CancellationToken token = new())
    {
        foreach (var classPeriod in models)
        {
            var existsClassPeriod = await _dbContext.ClassPeriods.Where(x => x.Name == classPeriod.Name)
                .SingleOrDefaultAsync(token);
            if (existsClassPeriod is null)
                await _dbContext.ClassPeriods.AddAsync(_mapper.Map<ClassPeriod>(classPeriod), token);
            else
                _mapper.Map(classPeriod, existsClassPeriod);
        }

        await _dbContext.SaveChangesAsync(token);
    }
}