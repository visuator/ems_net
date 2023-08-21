using AutoMapper;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ems.Domain.Services;

public class StudentService : IStudentService
{
    private readonly AccountOptions _accountOptions;
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordProvider _passwordProvider;

    public StudentService(EmsDbContext dbContext, IPasswordProvider passwordProvider,
        IOptions<AccountOptions> accountOptions, IMapper mapper)
    {
        _dbContext = dbContext;
        _passwordProvider = passwordProvider;
        _mapper = mapper;
        _accountOptions = accountOptions.Value;
    }

    public async Task Import(DateTime requestedAt, List<ExcelStudentModel> models, CancellationToken token = new())
    {
        foreach (var model in models)
        {
            var existsStudent = await _dbContext.Students.AsTracking().Include(x => x.Account).Where(x =>
                    x.FirstName == model.FirstName && x.LastName == model.LastName && x.MiddleName == model.MiddleName)
                .SingleOrDefaultAsync(token);
            var group = await _dbContext.Groups.Where(x => x.Name == model.GroupName).Select(x => new { x.Id })
                .SingleAsync(token);
            if (existsStudent is not null)
            {
                _mapper.Map(model, existsStudent, opt => opt.AfterMap((src, dst) => { dst.GroupId = group.Id; }));
                continue;
            }

            var password = _passwordProvider.GenerateRandomPassword();
            var passwordModel = HashHelper.HashPassword(password);
            var confirmationToken = HashHelper.GenerateRandomToken();
            var confirmationExpiration = requestedAt.Add(_accountOptions.LinkExpirationTime);

            var student = _mapper.Map<Student>(model, opt => opt.AfterMap((_, dst) =>
            {
                dst.Account.PasswordHash = passwordModel.PasswordHash;
                dst.Account.PasswordSalt = passwordModel.PasswordSalt;
                dst.Account.ConfirmationToken = confirmationToken;
                dst.Account.ConfirmationExpiresAt = confirmationExpiration;
                dst.GroupId = group.Id;
            }));
            var registrationEmail = new RegistrationEmail
            {
                Recipient = model.Email,
                ConfirmationToken = confirmationToken,
                ConfirmationExpiresAt = confirmationExpiration,
                Password = password,
                Status = EmailStatus.Created,
                Type = EmailType.Registration
            };

            await _dbContext.Students.AddAsync(student, token);
            await _dbContext.Emails.AddAsync(registrationEmail, token);
        }

        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Students.Where(x => x.Id == id).AnyAsync(token);
    }
}