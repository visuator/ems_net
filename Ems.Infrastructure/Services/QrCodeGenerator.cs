using Ems.Infrastructure.Constants;
using Ems.Infrastructure.Options;
using IronBarCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Ems.Infrastructure.Services;

public class QrCodeGenerator : IQrCodeGenerator
{
    private readonly QrCodeOptions _qrCodeOptions;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public QrCodeGenerator(IOptions<QrCodeOptions> qrCodeOptions, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        _qrCodeOptions = qrCodeOptions.Value;
    }

    public byte[] Get(string content, string logoFileName)
    {
        var directory = _webHostEnvironment.ContentRootFileProvider.GetDirectoryContents(
            Path.Combine(FileConstants.AppDataDirectory, FileConstants.ImagesDirectory));
        using var logo = directory.Single(x => x.Name == logoFileName).CreateReadStream();
        var qrCodeLogo = new QRCodeLogo(logo);
        return QRCodeWriter.CreateQrCodeWithLogo(content, qrCodeLogo, _qrCodeOptions.Size).ToPngBinaryData();
    }
}