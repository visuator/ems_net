using AutoMapper;
using AutoMapper.AspNet.OData;
using Ems.Core.Entities;
using Ems.Domain.Models;
using Ems.Infrastructure.Storages;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class ExternalAccountService : IExternalAccountService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExternalAccountService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<ExternalAccountDto>> GetAll(ODataQueryOptions<ExternalAccountDto> query,
        CancellationToken token = new())
    {
        return await _dbContext.ExternalAccounts.GetQuery(_mapper, query).ToListAsync(token);
    }

    public async Task AddExternalAccount(AddExternalAccountModel model, CancellationToken token = new())
    {
        var externalAccount = _mapper.Map<ExternalAccount>(model);

        await _dbContext.ExternalAccounts.AddAsync(externalAccount, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task DeleteExternalAccount(DeleteExternalAccountModel model, CancellationToken token = new())
    {
        var externalAccount =
            await _dbContext.ExternalAccounts.AsTracking().Where(x => x.Id == model.Id).SingleAsync(token);
        _dbContext.ExternalAccounts.Remove(externalAccount);
        await _dbContext.SaveChangesAsync(token);
    }
}