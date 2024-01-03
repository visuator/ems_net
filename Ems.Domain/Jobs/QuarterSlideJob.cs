using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;

namespace Ems.Domain.Jobs;

public class QuarterSlideJob : IJobBase
{
    public Guid SettingId { get; init; }
    public string Id => $"{SettingId}";

    public class QuartzHandler : IJob
    {
        private static readonly Dictionary<Quarter, Quarter> Map = new()
        {
            { Quarter.First, Quarter.Second },
            { Quarter.Second, Quarter.First },
        };
        private readonly EmsDbContext _dbContext;

        public QuartzHandler(EmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (context.MergedJobDataMap["model"] is not QuarterSlideJob model)
                throw new NullReferenceException();

            var setting = await _dbContext.Settings.AsTracking().Where(x => x.Id == model.SettingId).SingleAsync();

            setting.CurrentQuarter = Map[setting.CurrentQuarter];
            await _dbContext.SaveChangesAsync();
        }
    }

    public class ScheduleService : IScheduleService<QuarterSlideJob>
    {
        private readonly QuarterSlideOptions _quarterSlideOptions;
        private readonly ISchedulerFactory _schedulerFactory;

        public ScheduleService(ISchedulerFactory schedulerFactory,
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
}