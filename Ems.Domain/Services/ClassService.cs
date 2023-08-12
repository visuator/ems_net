using AutoMapper;
using Ems.Core.Entities;
using Ems.Infrastructure.Storages;
using Ems.Models;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class ClassService : IClassService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ClassService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task CreateReplacement(CreateReplacementModel model, CancellationToken token = new())
    {
        var sourceClass = await _dbContext.Classes.Where(x => x.Id == model.SourceClassId).SingleAsync(token);
        var copy = _mapper.Map<Class>(model, opt => opt.AfterMap((_, dst) =>
        {
            dst.TemplateId = sourceClass.TemplateId;
            dst.StartingAt = sourceClass.StartingAt;
            dst.EndingAt = sourceClass.EndingAt;
        }));

        await _dbContext.Classes.AddAsync(copy, token);
        await _dbContext.SaveChangesAsync(token);
    }
}