using Ems.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface IExternalAccountService
{
    Task<List<ExternalAccountDto>> GetAll(ODataQueryOptions<ExternalAccountDto> query, CancellationToken token = new());
    Task AddExternalAccount(AddExternalAccountModel model, CancellationToken token = new());
    Task DeleteExternalAccount(DeleteExternalAccountModel model, CancellationToken token = new());
}