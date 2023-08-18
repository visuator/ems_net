using AutoMapper;
using Ems.Core.Entities;
using Ems.Infrastructure.Storages;
using Ems.Models;

namespace Ems.Domain.Services;

public class StudentRecordService : IStudentRecordService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public StudentRecordService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Create(CreateGeolocationStudentRecordModel model, CancellationToken token = new())
    {
        var studentRecord = _mapper.Map<StudentRecord>(model);

        await _dbContext.StudentRecords.AddAsync(studentRecord, token);
        await _dbContext.SaveChangesAsync(token);
    }
}