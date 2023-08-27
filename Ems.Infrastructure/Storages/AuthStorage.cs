using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Services;

namespace Ems.Infrastructure.Storages;

public class AuthStorage : IAuthStorage
{
    public List<Role>? CurrentRoles { get; set; }
}