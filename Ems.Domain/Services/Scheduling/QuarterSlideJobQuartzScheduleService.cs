using Ems.Domain.Jobs;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Quartz;

namespace Ems.Domain.Services.Scheduling;

public class QuarterSlideJobQuartzScheduleService : IScheduleService<QuarterSlideJob>
{
    private readonly QuarterSlideOptions _quarterSlideOptions;
    private readonly ISchedulerFactory _schedulerFactory;

    public QuarterSlideJobQuartzScheduleService(ISchedulerFactory schedulerFactory,
        IOptions<QuarterSlideOptions> quarterSlideOptions)
    {
        _schedulerFactory = schedulerFactory;
        _quarterSlideOptions = quarterSlideOptions.Value;
    }

    public async Task ScheduleJob(QuarterSlideJob job, CancellationToken token = new())
    {
        var scheduler = await _schedulerFactory.GetScheduler(token);

        var jobDetails = JobBuilder.Create<QuarterSlideJob.QuartzHandler>()
            .WithIdentity($"{nameof(QuarterSlideJob)}-{job.Id}")
            .UsingJobData(new JobDataMap
            {
                { "model", job }
            })
            .Build();
        var periodicalTrigger = TriggerBuilder.Create()
            .WithIdentity($"periodical-{job!.Id}")
            .WithCronSchedule($"0 0 0 ? * {(int)_quarterSlideOptions.PublicationDay + 1}")
            .Build();

        await scheduler.ScheduleJob(jobDetails, new[] { periodicalTrigger }, false, token);
    }
}