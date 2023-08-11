namespace Ems.Infrastructure.Services;

public interface IScheduleService<in TJob>
{
    Task ScheduleJob(TJob job, CancellationToken token = new());
}