using System.Security.Claims;
using Application.Abstractions.Authentication;

namespace Library.Api.Authentication;

internal sealed class HttpContextCurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public string IdentityId => FindClaim("sub", ClaimTypes.NameIdentifier);

    public string Email => FindClaim("email", ClaimTypes.Email);

    public string FullName => FindClaim("name", ClaimTypes.Name);

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;

    private string FindClaim(string claimType, string fallbackClaimType) =>
        User?.FindFirst(claimType)?.Value ?? User?.FindFirst(fallbackClaimType)?.Value ?? string.Empty;
}
