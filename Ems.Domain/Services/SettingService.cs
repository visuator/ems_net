using AutoMapper;
using AutoMapper.AspNet.OData;
using Ems.Core.Entities;
using Ems.Domain.Jobs;
using Ems.Domain.Models;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storage;
using Ems.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ems.Domain.Services;

public class SettingService : ISettingService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly QrCodeStudentRecordSessionOptions _qrCodeStudentRecordSessionOptions;
    private readonly IScheduleService<QuarterSlideJob> _quarterSlideJobScheduleService;

    public SettingService(EmsDbContext dbContext, IScheduleService<QuarterSlideJob> quarterSlideJobScheduleService,
        IMapper mapper, IOptions<QrCodeStudentRecordSessionOptions> qrCodeStudentRecordSessionOptions)
    {
        _dbContext = dbContext;
        _quarterSlideJobScheduleService = quarterSlideJobScheduleService;
        _mapper = mapper;
        _qrCodeStudentRecordSessionOptions = qrCodeStudentRecordSessionOptions.Value;
    }

    public async Task<bool> AnyAsync(CancellationToken token = new())
    {
        return await _dbContext.Settings.AnyAsync(token);
    }

    public async Task<List<SettingDto>> GetAll(ODataQueryOptions<SettingDto> query, CancellationToken token = new())
    {
        return await _dbContext.Settings.GetQuery(_mapper, query).ToListAsync(token);
    }

    public async Task Create(CreateSettingModel model, CancellationToken token = new())
    {
        var setting = _mapper.Map<Setting>(model);
        await _dbContext.Settings.AddAsync(setting, token);
        await _dbContext.SaveChangesAsync(token);
        await _quarterSlideJobScheduleService.ScheduleJob(new QuarterSlideJob { SettingId = setting.Id }, token);
    }

    public Task<QrCodeStudentRecordSessionOptionsModel> GetQrCodeStudentRecordSessionOptions(
        CancellationToken token = new())
    {
        return Task.FromResult(_mapper.Map<QrCodeStudentRecordSessionOptionsModel>(_qrCodeStudentRecordSessionOptions));
    }
}