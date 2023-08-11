using Ems.Models;

namespace Ems.Infrastructure.Services;

public interface IEmailSender
{
    Task Send(SendEmailModel model, CancellationToken token = new());
}