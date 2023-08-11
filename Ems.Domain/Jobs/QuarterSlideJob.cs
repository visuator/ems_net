using Ems.Domain.Constants;
using Ems.Domain.Exceptions;
using Ems.Infrastructure.Storages;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Ems.Domain.Jobs;

public class QuarterSlideJob
{
    public Guid SettingId { get; init; }
    public string Id => $"{SettingId.ToString()}";

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
                if (context.MergedJobDataMap["model"] is not QuarterSlideJob model)
                    throw new NullModelJobException();

                var setting = await _dbContext.Settings.AsTracking().Where(x => x.Id == model.SettingId).SingleAsync();

                setting.CurrentQuarter = QuarterSlider.Map[setting.CurrentQuarter];
                await _dbContext.SaveChangesAsync();
            }
            catch (NullModelJobException e)
            {
                //TODO: log + error
            }
            catch (Exception e)
            {
                //TODO: log
            }
        }
    }
}