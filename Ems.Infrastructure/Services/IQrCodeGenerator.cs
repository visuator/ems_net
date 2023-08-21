namespace Ems.Infrastructure.Services;

public interface IQrCodeGenerator
{
    byte[] Get(string content, string logoFileName);
}