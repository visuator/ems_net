using Ems.Core.Entities;
using Ems.Domain.Exceptions;
using Ems.Infrastructure.Storages;
using Microsoft.EntityFrameworkCore;
using NCrontab;
using Quartz;

namespace Ems.Domain.Jobs;

public class PublishClassVersionJob
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
            try
            {
                if (context.MergedJobDataMap["model"] is not PublishClassVersionJob model)
                    throw new NullModelJobException();

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
                        throw new ClassDayDoesNotMatchJobException();
                    copy.StartingAt = scheduledStartingAt;
                    copy.EndingAt = scheduledEndingAt;

                    await _dbContext.Classes.AddAsync(copy);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (NullModelJobException e)
            {
                //TODO: log + error
            }
            catch (ClassDayDoesNotMatchJobException e)
            {
                //TODO: log + warn
            }
            catch (Exception e)
            {
                //TODO: log
            }
        }
    }
}