using Ems.Core.Entities;
using Ems.Domain.Models;
using Ems.Models;

namespace Ems.Domain.Services;

public interface IClassService
{
    Task CreateReplacement(CreateReplacementModel model, CancellationToken token = new());
    Task<List<GroupClassInfoModel>> GetGroupCurrent(GetGroupCurrentModel model, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
    Task<Class?> GetCurrent(Guid accountId, DateTime requestedAt, CancellationToken token = new());
}