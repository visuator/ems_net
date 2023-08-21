using AutoMapper;
using AutoMapper.AspNet.OData;
using Ems.Core.Entities;
using Ems.Domain.Models;
using Ems.Infrastructure.Storages;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class IdlePeriodService : IIdlePeriodService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public IdlePeriodService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Create(CreateIdlePeriodModel model, CancellationToken token = new())
    {
        var idlePeriod = _mapper.Map<IdlePeriod>(model);

        await _dbContext.IdlePeriods.AddAsync(idlePeriod, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<List<IdlePeriodDto>> GetAll(ODataQueryOptions<IdlePeriodDto> query,
        CancellationToken token = new())
    {
        return await _dbContext.IdlePeriods.GetQuery(_mapper, query).ToListAsync(token);
    }
}