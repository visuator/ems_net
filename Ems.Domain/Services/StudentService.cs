using AutoMapper;
using Ems.Infrastructure.Storage;
using Ems.Models.Excel;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public interface IStudentService
{
    Task Import(DateTime requestedAt, List<ExcelStudentModel> models, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
}
public class StudentService : IStudentService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public StudentService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Import(DateTime requestedAt, List<ExcelStudentModel> models, CancellationToken token = new())
    {
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Students.Where(x => x.Id == id).AnyAsync(token);
    }
}