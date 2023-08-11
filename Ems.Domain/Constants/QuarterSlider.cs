using Ems.Core.Entities.Enums;

namespace Ems.Domain.Constants;

public static class QuarterSlider
{
    public static Dictionary<Quarter, Quarter> Map => new()
    {
        { Quarter.First, Quarter.Second },
        { Quarter.Second, Quarter.First }
    };
}