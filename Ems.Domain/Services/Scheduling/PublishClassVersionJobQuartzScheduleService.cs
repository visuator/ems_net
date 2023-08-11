using Ems.Domain.Jobs;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Quartz;

namespace Ems.Domain.Services.Scheduling;

public class PublishClassVersionJobQuartzScheduleService : IScheduleService<PublishClassVersionJob>
{
    private readonly ClassVersionOptions _classVersionOptions;
    private readonly ISchedulerFactory _schedulerFactory;

    public PublishClassVersionJobQuartzScheduleService(ISchedulerFactory schedulerFactory,
        IOptions<ClassVersionOptions> classVersionOptions)
    {
        _schedulerFactory = schedulerFactory;
        _classVersionOptions = classVersionOptions.Value;
    }

    public async Task ScheduleJob(PublishClassVersionJob job, CancellationToken token = new())
    {
        var scheduler = await _schedulerFactory.GetScheduler(token);

        var jobDetails = JobBuilder.Create<PublishClassVersionJob.QuartzHandler>()
            .WithIdentity($"{nameof(PublishClassVersionJob)}-{job!.Id}")
            .UsingJobData(new JobDataMap
            {
                { "model", job }
            })
            .Build();
        var periodicalJobTrigger = TriggerBuilder.Create()
            .WithIdentity($"periodical-{job!.Id}")
            .WithPriority(1)
            .WithCronSchedule($"0 0 0 ? * {(int)_classVersionOptions.PublicationDay + 1}")
            .Build();
        var immediateJobTrigger = TriggerBuilder.Create()
            .WithIdentity($"immediate-{job!.Id}")
            .WithPriority(2)
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(jobDetails, new[] { periodicalJobTrigger, immediateJobTrigger }, false,
            token);
    }
}