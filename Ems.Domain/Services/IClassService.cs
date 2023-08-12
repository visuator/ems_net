using Ems.Models;

namespace Ems.Domain.Services;

public interface IClassService
{
    Task CreateReplacement(CreateReplacementModel model, CancellationToken token = new());
}