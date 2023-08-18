using AutoMapper;
using Ems.Core.Entities;
using Ems.Domain.Jobs;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ems.Domain.Services;

public class StudentRecordSessionService : IStudentRecordSessionService
{
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IScheduleService<GeolocationStudentRecordSessionJob> _gpsStudentRecordJobScheduleService;
    private readonly GeolocationStudentRecordSessionOptions _geolocationStudentRecordSessionOptions;

    public StudentRecordSessionService(EmsDbContext dbContext, IMapper mapper, IScheduleService<GeolocationStudentRecordSessionJob> gpsStudentRecordJobScheduleService, IOptions<GeolocationStudentRecordSessionOptions> studentRecordSessionGpsOptions)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _gpsStudentRecordJobScheduleService = gpsStudentRecordJobScheduleService;
        _geolocationStudentRecordSessionOptions = studentRecordSessionGpsOptions.Value;
    }

    public async Task<StudentRecordSession> Get(Guid id, CancellationToken token = new())
    {
        return await _dbContext.StudentRecordSessions.Where(x => x.Id == id).SingleAsync(token);
    }

    public async Task Create(CreateGeolocationStudentRecordSessionModel model, CancellationToken token = new ())
    {
        var studentRecordSession = _mapper.Map<StudentRecordSession>(model, opt => opt.AfterMap(
            (_, dst) =>
            {
                dst.EndingAt = model.RequestedAt.Add(_geolocationStudentRecordSessionOptions.Expiration);
            }));
        await _dbContext.StudentRecordSessions.AddAsync(studentRecordSession, token);
        await _dbContext.SaveChangesAsync(token);

        await _gpsStudentRecordJobScheduleService.ScheduleJob(_mapper.Map<GeolocationStudentRecordSessionJob>(studentRecordSession),
            token);
    }
}