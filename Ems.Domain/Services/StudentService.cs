using AutoMapper;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Storage;
using Ems.Models.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ems.Domain.Services;

public class StudentService : IStudentService
{
    private readonly AccountOptions _accountOptions;
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public StudentService(EmsDbContext dbContext,
        IOptions<AccountOptions> accountOptions, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _accountOptions = accountOptions.Value;
    }

    public async Task Import(DateTime requestedAt, List<ExcelStudentModel> models, CancellationToken token = new())
    {
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Students.Where(x => x.Id == id).AnyAsync(token);
    }
}