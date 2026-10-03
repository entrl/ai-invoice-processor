using InvoiceProcessor.Domain.Users;

namespace InvoiceProcessor.Application.Auth;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string HashRefreshToken(string refreshToken);
}