using Ems.Domain.Jobs;
using Ems.Infrastructure.Services;
using Quartz;

namespace Ems.Domain.Services.Scheduling;

public class StudentRecordJobQuartzScheduleService : IScheduleService<GeolocationStudentRecordSessionJob>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public StudentRecordJobQuartzScheduleService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task ScheduleJob(GeolocationStudentRecordSessionJob sessionJob, CancellationToken token = new())
    {
        var scheduler = await _schedulerFactory.GetScheduler(token);

        var jobDetails = JobBuilder.Create<GeolocationStudentRecordSessionJob.QuartzHandler>()
            .WithIdentity($"{nameof(GeolocationStudentRecordSessionJob)}-{sessionJob.Id}")
            .UsingJobData(new JobDataMap
            {
                { "model", sessionJob }
            }).Build();

        var immediateJobTrigger = TriggerBuilder.Create()
            .WithIdentity($"immediate-{sessionJob.Id}")
            .WithPriority(1)
            .StartAt(sessionJob.EndingAt)
            .Build();

        await scheduler.ScheduleJob(jobDetails, new[] { immediateJobTrigger }, false,
            token);
    }
}