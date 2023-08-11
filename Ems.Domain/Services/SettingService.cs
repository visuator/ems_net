using AutoMapper;
using AutoMapper.AspNet.OData;
using Ems.Core.Entities;
using Ems.Domain.Jobs;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class SettingService : ISettingService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IScheduleService<QuarterSlideJob> _quarterSlideJobScheduleService;

    public SettingService(EmsDbContext dbContext, IScheduleService<QuarterSlideJob> quarterSlideJobScheduleService,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _quarterSlideJobScheduleService = quarterSlideJobScheduleService;
        _mapper = mapper;
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
}