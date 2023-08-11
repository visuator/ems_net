namespace Ems.Models.Dtos;

public class RefreshTokenDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public AccountDto Account { get; set; }
    public Guid? SessionTokenId { get; set; }
    public RefreshTokenDto? SessionToken { get; set; }
    public string Value { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public DateTimeOffset? UsedAt { get; set; }
}