using System.Runtime.InteropServices.JavaScript;
using InvoiceProcessor.Application.Users;
using InvoiceProcessor.Domain.Users;

namespace InvoiceProcessor.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly int _refreshTokenExpirationDays;

    public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, int refreshTokenExpirationDays)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _refreshTokenExpirationDays = refreshTokenExpirationDays;
    }
    
    
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            throw new InvalidOperationException("A user with that email already exists");
        
        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new User(request.Email, passwordHash);
        
        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);
        
        return await IssueTokensAsync(user, ct);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            return null;
        
        return await IssueTokensAsync(user, ct);
    }

    public async Task<AuthResponse?> RefreshAsync(RefreshRequest request, CancellationToken ct = default)
    {
        var incomingHash = _jwtTokenService.HashRefreshToken(request.RefreshToken);
        var existingToken = await _refreshTokenRepository.GetByHashAsync(incomingHash, ct);

        if (existingToken is null || !existingToken.IsActive)
            return null;

        var (response, newRefreshToken) = await CreateTokenPairAsync(existingToken.User, ct);

        existingToken.Revoke(newRefreshToken.Id);
        await _refreshTokenRepository.SaveChangesAsync(ct);

        return response;
    }

    private async Task<AuthResponse> IssueTokensAsync(User user, CancellationToken ct)
    {
        var (response, _) = await CreateTokenPairAsync(user, ct);
        return response;
    }

    private async Task<(AuthResponse Response, RefreshToken Token)> CreateTokenPairAsync(User user, CancellationToken ct)
    {
        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var rawRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenHash = _jwtTokenService.HashRefreshToken(rawRefreshToken);

        var refreshToken = new RefreshToken(
            user,
            refreshTokenHash,
            DateTime.UtcNow.AddDays(_refreshTokenExpirationDays));

        await _refreshTokenRepository.AddAsync(refreshToken, ct);
        await _refreshTokenRepository.SaveChangesAsync(ct);

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = rawRefreshToken
        };

        return (response, refreshToken);
    }
}