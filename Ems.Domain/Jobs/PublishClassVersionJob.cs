using Ems.Core.Entities;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NCrontab;
using Quartz;

namespace Ems.Domain.Jobs;

public class PublishClassVersionJob : IJobBase
{
    public Guid ClassVersionId { get; set; }
    public DateTime RequestedAt { get; set; }
    public string Id => $"{ClassVersionId.ToString()}";

    public class QuartzHandler : IJob
    {
        private readonly EmsDbContext _dbContext;

        public QuartzHandler(EmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (context.MergedJobDataMap["model"] is not PublishClassVersionJob model)
                throw new NullReferenceException();

            var currentSetting = await _dbContext.Settings.OrderByDescending(x => x.CreatedAt).FirstAsync();
            var classes = await _dbContext.Classes.Include(x => x.ClassPeriod).Where(x =>
                    x.ClassVersionId == model.ClassVersionId && x.TemplateId == null &&
                    x.Quarter == currentSetting.CurrentQuarter)
                .ToListAsync();

            foreach (var @class in classes)
            {
                var copy = new Class
                {
                    TemplateId = @class.Id,
                    ClassroomId = @class.ClassroomId,
                    LecturerId = @class.LecturerId,
                    LessonId = @class.LessonId
                };

                var cron =
                    CrontabSchedule.Parse(
                        $"0 0 * * {(int)@class.Day!}");

                // Если следующее начало занятие разнится на день, после добавления к нему времени периода, то в таком случае это занятие проходит в полночь, что запрещено.
                DateTime onTime;
                if (context.Trigger.Key.Name == $"immediate-{model.Id}")
                    onTime = model.RequestedAt.Date;
                else if (context.Trigger.Key.Name == $"periodical-{model.Id}")
                    onTime = DateTime.UtcNow.Date;
                else
                    onTime = DateTime.UtcNow.Date;
                var startingAt = cron.GetNextOccurrence(onTime);
                var scheduledStartingAt = startingAt.Date.Add(@class.ClassPeriod!.StartingAt);
                var scheduledEndingAt = startingAt.Date.Add(@class.ClassPeriod!.EndingAt);

                if (scheduledStartingAt.Day != startingAt.Day || scheduledEndingAt.Day != startingAt.Day)
                    return;
                copy.StartingAt = scheduledStartingAt;
                copy.EndingAt = scheduledEndingAt;

                await _dbContext.Classes.AddAsync(copy);
            }

            await _dbContext.SaveChangesAsync();
        }
    }

    public class ScheduleService : IScheduleService<PublishClassVersionJob>
    {
        private readonly ClassVersionOptions _classVersionOptions;
        private readonly ISchedulerFactory _schedulerFactory;

        public ScheduleService(ISchedulerFactory schedulerFactory,
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
}