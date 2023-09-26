using AutoMapper;
using AutoMapper.AspNet.OData;
using AutoMapper.QueryableExtensions;
using EFCoreSecondLevelCacheInterceptor;
using Ems.Core.Entities;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;
using Ems.Domain.Extensions;
using Ems.Domain.Models;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class StudentRecordService : IStudentRecordService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthStorage _authStorage;

    public StudentRecordService(EmsDbContext dbContext, IMapper mapper, IAuthStorage authStorage)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authStorage = authStorage;
    }

    public async Task<List<StudentRecordDto>> GetAll(ODataQueryOptions<StudentRecordDto> query, CancellationToken token = default)
    {
        return await _dbContext.StudentRecords.GetQuery(_mapper, query).ODataMapFromRoles(_authStorage.CurrentRoles!).ToListAsync(token);
    }

    public async Task Create(CreateGeolocationStudentRecordModel model, CancellationToken token = new())
    {
        var studentRecord = _mapper.Map<StudentRecord>(model);

        await _dbContext.StudentRecords.AddAsync(studentRecord, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task Update(UpdateQrCodeStudentRecordStatusModel model, CancellationToken token = new())
    {
        var qrCodeAttempt = await _dbContext.QrCodeAttempts.NotCacheable().AsTracking()
            .Where(x => x.Content == model.Content)
            .SingleAsync(token);
        qrCodeAttempt.Status = QrCodeAttemptStatus.Passed;
        var qrCodeStudentRecordSession =
            await _dbContext.StudentRecordSessions.AsTracking()
                .Where(x => x.Id == qrCodeAttempt.QrCodeStudentRecordSessionId).SingleAsync(token);
        var studentRecord = _mapper.Map<QrCodeStudentRecord>(model, opt => opt.AfterMap(async (_, dst) =>
        {
            var student = await _dbContext.Students.Where(x => x.AccountId == model.AccountId).SingleAsync(token);
            dst.Status = StudentRecordStatus.OnTime;
            dst.StudentId = student.Id;
        }));
        qrCodeStudentRecordSession.StudentRecords.Add(studentRecord);
        qrCodeStudentRecordSession.EndingAt = model.RequestedAt;

        await _dbContext.SaveChangesAsync(token);
    }
}