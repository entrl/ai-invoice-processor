using InvoiceProcessor.Domain.Users;

namespace InvoiceProcessor.Application.Auth;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct = default);
    Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}