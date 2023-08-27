using Ems.Core.Entities.Enums;

namespace Ems.Infrastructure.Services;

public interface IAuthStorage
{
    List<Role>? CurrentRoles { get; set; }
}