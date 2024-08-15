using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRLeaveManagement.BlazorUI.Providers;

public sealed class ApiAuthenticationStateProvider(ILocalStorageService localStorage)
    : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage = localStorage;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var isTokenPresent = await _localStorage.ContainKeyAsync("token");

        if (!isTokenPresent) return new(user);

        var tokenContent = await GetTokenContent("token");

        if (tokenContent.ValidTo < DateTime.UtcNow)
        {
            await _localStorage.RemoveItemAsync("token");
            return new(user);
        }

        var claims = await GetClaims();
        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        return new(user);
    }

    public async Task LoggedIn()
    {
        var claims = await GetClaims();

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        var authState = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task LoggedOut()
    {
        await _localStorage.RemoveItemAsync("token");

        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = new AuthenticationState(nobody);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    private async Task<IEnumerable<Claim>> GetClaims()
    {
        var tokenContent = await GetTokenContent("token");

        var claims = tokenContent.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));

        return claims;
    }

    private async Task<JwtSecurityToken> GetTokenContent(string key)
    {
        var token = await _localStorage.GetItemAsync<string>(key);
        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);

        return tokenContent;
    }
}
