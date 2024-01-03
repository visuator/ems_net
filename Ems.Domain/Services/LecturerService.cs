using AutoMapper;
using AutoMapper.AspNet.OData;
using Ems.Infrastructure.Storage;
using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public interface ILecturerService
{
    Task Import(DateTime requestedAt, List<ExcelLecturerModel> models, CancellationToken token = new());
    Task<List<LecturerDto>> GetAll(ODataQueryOptions<LecturerDto> query, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
}
public class LecturerService : ILecturerService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public LecturerService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Import(DateTime requestedAt, List<ExcelLecturerModel> models, CancellationToken token = new())
    {
    }

    public async Task<List<LecturerDto>> GetAll(ODataQueryOptions<LecturerDto> query, CancellationToken token = new())
    {
        return await _dbContext.Lecturers.GetQuery(_mapper, query).ToListAsync(token);
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Lecturers.Where(x => x.Id == id).AnyAsync(token);
    }
}