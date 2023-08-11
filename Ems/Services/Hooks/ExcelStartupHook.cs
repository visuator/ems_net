using System.Text;

namespace Ems.Services.Hooks;

public class ExcelStartupHook : IStartupHook
{
    public Task Execute()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        return Task.CompletedTask;
    }
}