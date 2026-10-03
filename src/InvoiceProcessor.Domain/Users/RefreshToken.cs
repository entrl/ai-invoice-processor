namespace InvoiceProcessor.Domain.Users;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    public string TokenHash { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public Guid? ReplacedByTokenId { get; private set; }

    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;
    
    private  RefreshToken()
    {
        User = null!;
        TokenHash = null!;
    }

    public RefreshToken(User user, string tokenHash, DateTime expiresAt)
    {
        UserId = user.Id;
        User = user;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public void Revoke(Guid? replacedByTokenId = null)
    {
        RevokedAt = DateTime.UtcNow;
        ReplacedByTokenId = replacedByTokenId;
    }
}