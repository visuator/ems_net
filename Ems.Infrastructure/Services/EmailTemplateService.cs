using Ems.Infrastructure.Constants;
using Microsoft.AspNetCore.Hosting;

namespace Ems.Infrastructure.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EmailTemplateService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> GenerateBodyFromTemplate(string templateFileName, Dictionary<string, string> placeholders,
        CancellationToken token = new())
    {
        var emailTemplatesFileProvider =
            _webHostEnvironment.ContentRootFileProvider.GetDirectoryContents(
                Path.Combine(FileConstants.AppDataDirectory, FileConstants.EmailTemplatesDirectory));
        var file = emailTemplatesFileProvider.Single(x => x.Name == templateFileName);
        using var reader = new StreamReader(file.CreateReadStream());
        var content = await reader.ReadToEndAsync(token);
        foreach (var placeholder in placeholders) content = content.Replace($"[{placeholder.Key}]", placeholder.Value);

        return content;
    }
}