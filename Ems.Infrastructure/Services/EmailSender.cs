using System.Net;
using System.Net.Mail;
using Ems.Infrastructure.Exceptions;
using Ems.Infrastructure.Options;
using Ems.Models;
using Microsoft.Extensions.Options;

namespace Ems.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSenderOptions _emailSenderOptions;
    private readonly SmtpClient _smtpClient;

    public EmailSender(IOptions<EmailSenderOptions> emailSenderOptions)
    {
        _emailSenderOptions = emailSenderOptions.Value;
        _smtpClient = new SmtpClient(_emailSenderOptions.SmtpHost, _emailSenderOptions.SmtpPort);

        _smtpClient.Credentials =
            new NetworkCredential(_emailSenderOptions.Username, _emailSenderOptions.Password);
        _smtpClient.EnableSsl = true;
    }

    public async Task Send(SendEmailModel model, CancellationToken token = new())
    {
        var mailMessage = new MailMessage(_emailSenderOptions.From, model.Recipient, model.Subject, model.Body)
        {
            IsBodyHtml = true
        };
        try
        {
            await _smtpClient.SendMailAsync(mailMessage, token);
        }
        catch (Exception e)
        {
            throw new EmailSenderException(
                $"Ошибка отправки сообщения '{model.Subject}' по адресу '{model.Recipient}'");
        }
    }
}