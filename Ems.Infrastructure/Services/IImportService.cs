namespace Ems.Infrastructure.Services;

public interface IImportService
{
    Task Import(Stream file, DateTime? requestedAt = default, CancellationToken token = new());
}