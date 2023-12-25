using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
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
}