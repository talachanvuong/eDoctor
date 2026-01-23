using eDoctor.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace eDoctor.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LoginAsync(string loginName, string role)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginName),
            new Claim(ClaimTypes.Role, role)
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new Exception("HTTP context not found");
        }

        await httpContext.SignInAsync(principal);
    }

    public async Task LogoutAsync()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new Exception("HTTP context not found");
        }

        await httpContext.SignOutAsync();
    }
}
