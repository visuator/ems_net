namespace Ems.Infrastructure.Services;

public interface IImportService
{
    Task Import(Stream file, CancellationToken token = new());
}