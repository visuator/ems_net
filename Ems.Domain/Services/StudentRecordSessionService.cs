using AutoMapper;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Domain.Jobs;
using Ems.Domain.Models;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ems.Domain.Services;

public class StudentRecordSessionService : IStudentRecordSessionService
{
    private readonly EmsDbContext _dbContext;
    private readonly GeolocationStudentRecordSessionOptions _geolocationStudentRecordSessionOptions;
    private readonly IScheduleService<GeolocationStudentRecordSessionJob> _gpsStudentRecordJobScheduleService;
    private readonly IMapper _mapper;
    private readonly IQrCodeGenerator _qrCodeGenerator;
    private readonly QrCodeStudentRecordSessionOptions _qrCodeStudentRecordSessionOptions;

    public StudentRecordSessionService(EmsDbContext dbContext, IMapper mapper,
        IScheduleService<GeolocationStudentRecordSessionJob> gpsStudentRecordJobScheduleService,
        IOptions<GeolocationStudentRecordSessionOptions> studentRecordSessionGpsOptions,
        IOptions<QrCodeStudentRecordSessionOptions> qrCodeStudentRecordSessionOptions, IQrCodeGenerator qrCodeGenerator)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _gpsStudentRecordJobScheduleService = gpsStudentRecordJobScheduleService;
        _qrCodeGenerator = qrCodeGenerator;
        _qrCodeStudentRecordSessionOptions = qrCodeStudentRecordSessionOptions.Value;
        _geolocationStudentRecordSessionOptions = studentRecordSessionGpsOptions.Value;
    }

    public async Task Create(CreateGeolocationStudentRecordSessionModel model, CancellationToken token = new())
    {
        var studentRecordSession = _mapper.Map<GeolocationStudentRecordSession>(model, opt => opt.AfterMap(
            (_, dst) => { dst.EndingAt = model.RequestedAt.Add(_geolocationStudentRecordSessionOptions.Expiration); }));
        await _dbContext.StudentRecordSessions.AddAsync(studentRecordSession, token);
        await _dbContext.SaveChangesAsync(token);

        // отправлять уведомления вот здесь
        await _gpsStudentRecordJobScheduleService.ScheduleJob(
            _mapper.Map<GeolocationStudentRecordSessionJob>(studentRecordSession),
            token);
    }

    public async Task Create(CreateQrCodeStudentRecordSessionModel sessionModel, CancellationToken token = new())
    {
        var studentRecordSession = _mapper.Map<QrCodeStudentRecordSession>(sessionModel, opt => opt.AfterMap(
            (_, dst) =>
            {
                var id = Guid.NewGuid();
                dst.Id = id;
                dst.EndingAt = sessionModel.RequestedAt.Add(_qrCodeStudentRecordSessionOptions.Expiration);
                dst.Attempts = new List<QrCodeAttempt>();

                for (var i = 0; i < _qrCodeStudentRecordSessionOptions.MaxAttempts; i++)
                {
                    //var content = $"{id}-{HashHelper.GenerateRandomToken()}";
                    var content = "";
                    var image = _qrCodeGenerator.Get(content, _qrCodeStudentRecordSessionOptions.LogoFileName);
                    dst.Attempts.Add(new QrCodeAttempt
                    {
                        Content = content,
                        Image = image,
                        Status = QrCodeAttemptStatus.Created
                    });
                }
            }));
        await _dbContext.StudentRecordSessions.AddAsync(studentRecordSession, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.StudentRecordSessions.Where(x => x.Id == id).AnyAsync(token);
    }
}