using AutoMapper;
using AutoMapper.AspNet.OData;
using AutoMapper.QueryableExtensions;
using EFCoreSecondLevelCacheInterceptor;
using Ems.Core.Entities;
using Ems.Domain.Models;
using Ems.Infrastructure.Storage;
using Ems.Models;
using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public interface IGroupService
{
    Task Import(List<ExcelGroupModel> models, CancellationToken token = new());
    Task<List<GroupDto>> GetAll(ODataQueryOptions<GroupDto> query, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
    Task<CurrentGroupInfoModel> GetGroupInfo(GetGroupInfoModel model, CancellationToken token = new());
}
public class GroupService : IGroupService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public GroupService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Import(List<ExcelGroupModel> models, CancellationToken token = new())
    {
        foreach (var group in models)
        {
            var existsGroup = await _dbContext.Groups.NotCacheable().Where(x => x.Name == group.Name)
                .SingleOrDefaultAsync(token);
            if (existsGroup is null)
                await _dbContext.Groups.AddAsync(_mapper.Map<Group>(group), token);
            else
                _mapper.Map(group, existsGroup);
        }

        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<List<GroupDto>> GetAll(ODataQueryOptions<GroupDto> query, CancellationToken token = new())
    {
        return await _dbContext.Groups.GetQuery(_mapper, query).ToListAsync(token);
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Groups.Where(x => x.Id == id).AnyAsync(token);
    }

    public async Task<CurrentGroupInfoModel> GetGroupInfo(GetGroupInfoModel model, CancellationToken token = new())
    {
        var group = await _dbContext.Groups
            .Where(x => x.Id == model.Id)
            .ProjectTo<CurrentGroupInfoModel>(_mapper.ConfigurationProvider)
            .SingleAsync(token);

        return group;
    }
}