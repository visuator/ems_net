using EFCoreSecondLevelCacheInterceptor;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Exceptions;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;

namespace Ems.Domain.Services;

public class EmailBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EmailBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EmsDbContext>();
            var emailTemplateService = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            var emailOptions = scope.ServiceProvider.GetRequiredService<IOptions<EmailOptions>>().Value;
            var emailSenderOptions = scope.ServiceProvider.GetRequiredService<IOptions<EmailSenderOptions>>().Value;
            var urlService = scope.ServiceProvider.GetRequiredService<IUrlService>();

            var retryPolicy = Policy.Handle<EmailSenderException>().RetryAsync(emailSenderOptions.RetryingCount);

            var emails = await dbContext.Emails.Cacheable(CacheExpirationMode.Sliding, TimeSpan.FromSeconds(15))
                .AsTracking().Where(x => x.Status == EmailStatus.Created)
                .ToListAsync(token);
            foreach (var email in emails)
            {
                var fallbackPolicy = Policy.Handle<EmailSenderException>().FallbackAsync(_ =>
                {
                    email.Status = EmailStatus.Error;
                    return Task.CompletedTask;
                });
                var combinedPolicy = retryPolicy.WrapAsync(fallbackPolicy);

                var model = new SendEmailModel
                {
                    Recipient = email.Recipient
                };
                switch (email)
                {
                    case ReconfirmationEmail reconfirmationEmail:
                    {
                        var placeholders = new Dictionary<string, string>
                        {
                            {
                                "confirmationLink",
                                urlService.GenerateConfirmationLink(reconfirmationEmail.ConfirmationToken)
                            },
                            { "confirmationExpiresAt", reconfirmationEmail.ConfirmationExpiresAt.ToString("G") }
                        };
                        var body = await emailTemplateService.GenerateBodyFromTemplate(
                            emailOptions.ReconfirmationEmailTemplateFileName,
                            placeholders, token);

                        model.Subject = emailOptions.ReconfirmationEmailSubject;
                        model.Body = body;
                    }
                        break;
                    case RegistrationEmail registrationEmail:
                    {
                        var placeholders = new Dictionary<string, string>
                        {
                            {
                                "confirmationLink",
                                urlService.GenerateConfirmationLink(registrationEmail.ConfirmationToken)
                            },
                            { "confirmationExpiresAt", registrationEmail.ConfirmationExpiresAt.ToString("G") },
                            { "password", registrationEmail.Password }
                        };
                        var body = await emailTemplateService.GenerateBodyFromTemplate(
                            emailOptions.RegistrationEmailTemplateFileName,
                            placeholders, token);

                        model.Subject = emailOptions.RegistrationEmailSubject;
                        model.Body = body;
                    }
                        break;
                    case PasswordResetEmail passwordResetEmail:
                    {
                        var placeholders = new Dictionary<string, string>
                        {
                            {
                                "passwordResetLink",
                                urlService.GeneratePasswordResetLink(passwordResetEmail.PasswordResetToken)
                            },
                            { "passwordResetExpiresAt", passwordResetEmail.PasswordResetExpiresAt.ToString("G") }
                        };
                        var body = await emailTemplateService.GenerateBodyFromTemplate(
                            emailOptions.PasswordResetEmailTemplateFileName,
                            placeholders, token);

                        model.Subject = emailOptions.PasswordResetEmailSubject;
                        model.Body = body;
                    }
                        break;
                    case NewPasswordEmail newPasswordEmail:
                    {
                        var placeholders = new Dictionary<string, string>
                        {
                            { "password", newPasswordEmail.Password }
                        };
                        var body = await emailTemplateService.GenerateBodyFromTemplate(
                            emailOptions.NewPasswordEmailTemplateFileName, placeholders, token);

                        model.Subject = emailOptions.NewPasswordEmailSubject;
                        model.Body = body;
                    }
                        break;
                }

                await combinedPolicy.ExecuteAsync(async () =>
                {
                    await emailSender.Send(model, token);
                    email.Status = EmailStatus.Sent;
                });

                await dbContext.SaveChangesAsync(token);
            }

            await Task.Delay(TimeSpan.FromSeconds(emailSenderOptions.DelaySeconds), token);
        }
    }
}