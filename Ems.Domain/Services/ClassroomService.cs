using AutoMapper;
using AutoMapper.AspNet.OData;
using Ems.Core.Entities;
using Ems.Infrastructure.Storages;
using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class ClassroomService : IClassroomService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ClassroomService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Import(List<ExcelClassroomModel> models, CancellationToken token = new())
    {
        foreach (var classroom in models)
        {
            var existsClassroom =
                await _dbContext.Classrooms.Where(x => x.Name == classroom.Name).SingleOrDefaultAsync(token);
            if (existsClassroom is null)
                await _dbContext.Classrooms.AddAsync(_mapper.Map<Classroom>(classroom), token);
        }

        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<List<ClassroomDto>> GetAll(ODataQueryOptions<ClassroomDto> query, CancellationToken token = new())
    {
        return await _dbContext.Classrooms.GetQuery(_mapper, query).ToListAsync(token);
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Classrooms.Where(x => x.Id == id).AnyAsync(token);
    }
}