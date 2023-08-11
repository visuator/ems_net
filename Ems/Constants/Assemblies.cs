using System.Reflection;

namespace Ems.Constants;

public static class Assemblies
{
    public const string Infrastructure = "Ems.Infrastructure";
    public static Assembly Domain = Assembly.Load("Ems.Domain");
}