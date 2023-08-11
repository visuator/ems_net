namespace Ems.Infrastructure.Services;

public interface IEmailTemplateService
{
    Task<string> GenerateBodyFromTemplate(string templateFileName, Dictionary<string, string> placeholders,
        CancellationToken token = new());
}