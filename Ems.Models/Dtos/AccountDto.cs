namespace Ems.Models.Dtos;

public class AccountDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? ConfirmationExpiresAt { get; set; }
    public string? ConfirmationToken { get; set; }
    public DateTime? PasswordResetExpiresAt { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? LockExpiresAt { get; set; }
    public int FailedAttempts { get; set; }
    public ICollection<AccountRoleDto> Roles { get; set; }
    public ICollection<RefreshTokenDto> RefreshTokens { get; set; }
    public ICollection<ExternalAccountDto> ExternalAccounts { get; set; }
}