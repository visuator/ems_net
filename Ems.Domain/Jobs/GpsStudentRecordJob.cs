using Ems.Domain.Exceptions;
using Ems.Domain.Services;
using Ems.Infrastructure.Storages;
using Quartz;

namespace Ems.Domain.Jobs;

public class GpsStudentRecordJob
{
    public Guid SessionId { get; set; }
    public DateTime EndingAt { get; set; }
    public string Id => $"{SessionId.ToString()}";
    
    public class GpsQuartzHandler : IJob
    {
        private readonly EmsDbContext _dbContext;

        public GpsQuartzHandler(EmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (context.MergedJobDataMap["model"] is not GpsStudentRecordJob model)
                    throw new NullModelJobException();
            }
            catch (NullModelJobException e)
            {
                //TODO: Log error
            }
        }
    }
}