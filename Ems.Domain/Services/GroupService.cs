using AutoMapper;
using AutoMapper.AspNet.OData;
using AutoMapper.QueryableExtensions;
using Ems.Core.Entities;
using Ems.Domain.Enums;
using Ems.Infrastructure.Storages;
using Ems.Models;
using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

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
            var existsGroup = await _dbContext.Groups.Where(x => x.Name == group.Name).SingleOrDefaultAsync(token);
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
        var idlePeriods = await _dbContext.IdlePeriods
            .OrderByDescending(x => x.CreatedAt)
            .Where(x => x.StartingAt >= model.RequestedAt && model.RequestedAt <= x.EndingAt)
            .Where(x => x.GroupId == model.Id || x.GroupId == null)
            .ToListAsync(token);
        var classes = _dbContext.Classes
            .Include(x => x.Lecturer)
            .Include(x => x.Group)
            .Include(x => x.Lesson)
            .Include(x => x.Classroom)
            .Where(x => x.GroupId == model.Id).Where(x => x.TemplateId == null)
            .Where(x => x.StartingAt!.Value.Date == model.RequestedAt.Date)
            .AsAsyncEnumerable();
        await foreach(var @class in classes.WithCancellation(token))
        {
            var shouldIgnore =
                idlePeriods.Any(ip => @class.StartingAt >= ip.StartingAt && @class.EndingAt <= ip.EndingAt);
            if (shouldIgnore) continue;
            group.Classes.Add(_mapper.Map<GroupClassInfoModel>(@class, opt => opt.AfterMap((_, dst) =>
            {
                if (@class.StartingAt >= model.RequestedAt && model.RequestedAt <= @class.EndingAt)
                    dst.Status = GroupClassStatus.Current;
                else if (@class.StartingAt >= model.RequestedAt)
                    dst.Status = GroupClassStatus.Next;
                else dst.Status = GroupClassStatus.Previous;
            })));
        }

        return group;
    }
}