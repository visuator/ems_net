namespace Ems.Services.Hooks;

public interface IStartupHook
{
    Task Execute();
}