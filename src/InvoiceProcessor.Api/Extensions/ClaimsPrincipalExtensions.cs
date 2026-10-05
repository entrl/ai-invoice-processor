using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InvoiceProcessor.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                    ?? throw new InvalidOperationException("User id claim is missing.");
        
        return Guid.Parse(value);
    }
}