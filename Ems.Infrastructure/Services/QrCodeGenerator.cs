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
        var logoPath = Path.Combine(_webHostEnvironment.ContentRootPath, FileConstants.AppDataDirectory,
            FileConstants.ImagesDirectory, logoFileName);
        using var logoStream = File.OpenRead(logoPath);
        var qrCodeLogo = new QRCodeLogo(logoStream);
        return QRCodeWriter
            .CreateQrCodeWithLogo(content, qrCodeLogo, _qrCodeOptions.ImageSize)
            .ToPngBinaryData();
    }
}