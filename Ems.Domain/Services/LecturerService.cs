﻿using AutoMapper;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ems.Domain.Services;

public class LecturerService : ILecturerService
{
    private readonly AccountOptions _accountOptions;
    private readonly EmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordProvider _passwordProvider;

    public LecturerService(EmsDbContext dbContext, IPasswordProvider passwordProvider,
        IOptions<AccountOptions> accountOptions, IMapper mapper)
    {
        _dbContext = dbContext;
        _passwordProvider = passwordProvider;
        _mapper = mapper;
        _accountOptions = accountOptions.Value;
    }

    public async Task Import(List<ExcelLecturerModel> models, CancellationToken token = new())
    {
        var password = _passwordProvider.GenerateRandomPassword();
        var passwordModel = HashHelper.HashPassword(password);
        var confirmationToken = HashHelper.GenerateRandomToken();
        var confirmationExpiration = DateTimeOffset.UtcNow.Add(_accountOptions.LinkExpirationTime);
        foreach (var model in models)
        {
            var existsLecturer = await _dbContext.Lecturers.AsTracking().Include(x => x.Account).Where(x =>
                    x.FirstName == model.FirstName && x.LastName == model.LastName && x.MiddleName == model.MiddleName)
                .SingleOrDefaultAsync(token);
            if (existsLecturer is not null) _mapper.Map(model, existsLecturer);

            var lecturer = _mapper.Map<Lecturer>(model, opt => opt.AfterMap((src, dst) =>
            {
                dst.Account.PasswordHash = passwordModel.PasswordHash;
                dst.Account.PasswordSalt = passwordModel.PasswordSalt;
                dst.Account.ConfirmationToken = confirmationToken;
                dst.Account.ConfirmationExpiresAt = confirmationExpiration;
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

            await _dbContext.Lecturers.AddAsync(lecturer, token);
            await _dbContext.Emails.AddAsync(registrationEmail, token);
        }

        await _dbContext.SaveChangesAsync(token);
    }
}