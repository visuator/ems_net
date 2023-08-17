using Ems.Domain.Jobs;
using Ems.Infrastructure.Services;
using Quartz;

namespace Ems.Domain.Services.Scheduling;

public class StudentRecordJobQuartzScheduleService : IScheduleService<GpsStudentRecordJob>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public StudentRecordJobQuartzScheduleService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task ScheduleJob(GpsStudentRecordJob job, CancellationToken token = new())
    {
        var scheduler = await _schedulerFactory.GetScheduler(token);
        
        var jobDetails = JobBuilder.Create<GpsStudentRecordJob.GpsQuartzHandler>()
            .WithIdentity($"{nameof(GpsStudentRecordJob)}-{job.Id}")
            .UsingJobData(new JobDataMap()
            {
                { "model", job }
            }).Build();

        var immediateJobTrigger = TriggerBuilder.Create()
            .WithIdentity($"immediate-{job.Id}")
            .WithPriority(1)
            .StartAt(job.EndingAt)
            .Build();
        
        await scheduler.ScheduleJob(jobDetails, new[] { immediateJobTrigger }, false,
            token);
    }
}