using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Domain.Services;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;

namespace Ems.Domain.Jobs;

public class GeolocationStudentRecordSessionJob : IJobBase
{
    public enum ArrayType
    {
        Minority,
        Majority
    }

    public Guid GeolocationStudentRecordSessionId { get; set; }
    public DateTime EndingAt { get; set; }
    public string Id => $"{GeolocationStudentRecordSessionId.ToString()}";

    private static List<(Guid StudentRecordId, int Distance)> CalculateDistance(
        IEnumerable<(Guid StudentRecordId, double Latitude, double Longitude)> studentRecords,
        double latitudeSource, double longitudeSource)
    {
        return studentRecords.Select(x => (x.StudentRecordId,
            MathService.HaversineDistance(latitudeSource, longitudeSource, x.Latitude, x.Longitude))).ToList();
    }

    private static Dictionary<ArrayType, List<Guid>> GetMajorityAndMinority(
        ICollection<(Guid StudentRecordId, int Distance)> data, int threshold)
    {
        var medianValue = (int)Math.Floor(data.Select(x => x.Distance).ToList().Median());
        var standardDeviation = (int)Math.Floor(data.Select(x => x.Distance).ToList().StandardDeviation());

        var majorityArray = data.Where(x => x.Distance <= medianValue).ToList();
        majorityArray.AddRange(data.Where(x => x.Distance <= threshold * standardDeviation));
        var minorityArray = data.Where(x => majorityArray.All(ma => ma.Distance != x.Distance)).ToList();

        return new Dictionary<ArrayType, List<Guid>>
        {
            { ArrayType.Majority, majorityArray.OrderBy(x => x.Distance).Select(x => x.StudentRecordId).ToList() },
            { ArrayType.Minority, minorityArray.OrderBy(x => x.Distance).Select(x => x.StudentRecordId).ToList() }
        };
    }

    public class QuartzHandler : IJob
    {
        private readonly EmsDbContext _dbContext;
        private readonly GeolocationStudentRecordSessionOptions _geolocationStudentRecordSessionOptions;

        public QuartzHandler(EmsDbContext dbContext,
            IOptions<GeolocationStudentRecordSessionOptions> geolocationStudentRecordSessionOptions)
        {
            _dbContext = dbContext;
            _geolocationStudentRecordSessionOptions = geolocationStudentRecordSessionOptions.Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            if (context.MergedJobDataMap["model"] is not GeolocationStudentRecordSessionJob model)
                throw new NullReferenceException();

            var session = await _dbContext.StudentRecordSessions.AsTracking()
                .OfType<GeolocationStudentRecordSession>()
                .Where(x => x.Id == model.GeolocationStudentRecordSessionId).Include(x =>
                    x.StudentRecords.Where(sr => sr.Status == StudentRecordStatus.Created)).SingleAsync();
            var geolocationStudentRecords = session.StudentRecords.OfType<GeolocationStudentRecord>()
                .Select(x => (x.Id, x.Latitude, x.Longitude)).ToList();
            var distances =
                CalculateDistance(
                    geolocationStudentRecords, session.Latitude,
                    session.Longitude);

            var arrays = GetMajorityAndMinority(distances, _geolocationStudentRecordSessionOptions.Threshold);
            foreach (var studentRecordId in arrays[ArrayType.Majority])
            {
                var studentRecord = session.StudentRecords.Single(x => x.Id == studentRecordId);
                studentRecord.Status = StudentRecordStatus.OnTime;
            }

            foreach (var studentRecordId in arrays[ArrayType.Minority])
            {
                var studentRecord = session.StudentRecords.Single(x => x.Id == studentRecordId);
                studentRecord.Status = StudentRecordStatus.Passed;
            }

            await _dbContext.SaveChangesAsync();

        }
    }

    public class ScheduleService : IScheduleService<GeolocationStudentRecordSessionJob>
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public ScheduleService(ISchedulerFactory schedulerFactory)
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
}