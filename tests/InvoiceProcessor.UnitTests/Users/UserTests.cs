using InvoiceProcessor.Domain.Users;

namespace InvoiceProcessor.UnitTests.Users;

public class UserTests
{
    [Fact]
    public void Constructor_SetsCreatedAt()
    {
        var before = DateTime.UtcNow;
        var user = new User("mail@mail.com",  "password");
        var after = DateTime.UtcNow;
        
        Assert.InRange(user.CreatedAt , before, after);
    }

    [Fact]
    public void Constructor_SetsUserRoleToUser()
    {
        var user = new User("mail@mail.com", "password");
        
        Assert.Equal(UserRole.User, user.Role);
    }

    [Fact]
    public void UpdatePasswordHash_UpdatesPasswordHash()
    {
        var user = new User("mail@mail.com", "password");
        user.UpdatePasswordHash("newpassword");
        
        Assert.Equal("newpassword", user.PasswordHash);
    }
}