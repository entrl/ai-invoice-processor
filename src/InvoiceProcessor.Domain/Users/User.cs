namespace InvoiceProcessor.Domain.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User()
    {
        Email = null!;
        PasswordHash = null!;
    }

    public User(string email, string passwordHash, UserRole role = UserRole.User)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void UpdatePasswordHash(string newHash)
    {
        PasswordHash = newHash;
    }
}