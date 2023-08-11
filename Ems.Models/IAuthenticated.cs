namespace Ems.Models;

public interface IAuthenticated
{
    Guid AccountId { get; set; }
}