using Ems.Models;

namespace Ems.Domain.Services;

public interface IClassService
{
    Task CreateReplacement(CreateReplacementModel model, CancellationToken token = new());
    Task<List<GroupClassInfoModel>> GetGroupCurrent(GetGroupCurrentModel model, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
}