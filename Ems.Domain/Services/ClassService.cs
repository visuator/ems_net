using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ems.Core.Entities;
using Ems.Domain.Extensions;
using Ems.Domain.Models;
using Ems.Infrastructure.Storages;
using Ems.Models;
using Ems.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Services;

public class ClassService : IClassService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ClassService(EmsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task CreateReplacement(CreateReplacementModel model, CancellationToken token = new())
    {
        var sourceClass = await _dbContext.Classes.Where(x => x.Id == model.SourceClassId).SingleAsync(token);
        var copy = _mapper.Map<Class>(model, opt => opt.AfterMap((_, dst) =>
        {
            dst.TemplateId = sourceClass.TemplateId;
            dst.StartingAt = sourceClass.StartingAt;
            dst.EndingAt = sourceClass.EndingAt;
        }));

        await _dbContext.Classes.AddAsync(copy, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<List<GroupClassInfoModel>> GetGroupCurrent(GetGroupCurrentModel model,
        CancellationToken token = new())
    {
        var setting = await _dbContext.Settings.OrderByDescending(x => x.CreatedAt).FirstAsync(token);
        var idlePeriods = await _dbContext.IdlePeriods
            .OrderByDescending(x => x.CreatedAt)
            .Where(x => x.StartingAt >= model.RequestedAt && model.RequestedAt <= x.EndingAt)
            .Where(x => x.GroupId == model.GroupId || x.GroupId == null)
            .ToListAsync(token);
        var classes = await _dbContext.Classes
            .Include(x => x.Template)
            .ThenInclude(x => x!.Group)
            .Where(x => x.TemplateId != null)
            .Where(x => x.Template!.GroupId == model.GroupId)
            .Where(x => x.Template!.Quarter == setting.CurrentQuarter)
            .Where(x => x.StartingAt!.Value.Date == model.RequestedAt.Date)
            .ProjectTo<GroupClassInfoModel>(_mapper.ConfigurationProvider, x => x.Classroom, x => x.Lesson,
                x => x.Lecturer)
            .GroupBy(x => new { x.TemplateId })
            .Select(x => x.OrderByDescending(c => c.CreatedAt).First())
            .ToListAsync(token);

        return classes
            .Where(x => !idlePeriods.Any(ip => x.StartingAt >= ip.StartingAt && x.EndingAt <= ip.EndingAt))
            .Select(x =>
            {
                if (x.StartingAt >= model.RequestedAt && model.RequestedAt <= x.EndingAt)
                    x.Status = GroupClassStatus.Current;
                else if (x.StartingAt >= model.RequestedAt)
                    x.Status = GroupClassStatus.Next;
                else x.Status = GroupClassStatus.Previous;

                return x;
            }).ToList();
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Classes.Where(x => x.Id == id).AnyAsync(token);
    }

    public async Task<Class?> GetCurrent(Guid accountId, DateTime requestedAt, CancellationToken token = new())
    {
        var accountRoles = await _dbContext.AccountRoles.Where(x => x.AccountId == accountId).Select(x => x.Role)
            .ToListAsync(token);
        return await _dbContext.Classes.ResolveByAccountRoles(accountRoles, accountId)
            .Where(x => requestedAt >= x.StartingAt && requestedAt <= x.EndingAt)
            .SingleOrDefaultAsync(token);
    }
}