using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace StudentAllowanceTracker.Client.Security
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            token = token?.Trim('"');

            if (string.IsNullOrWhiteSpace(token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var handler = new JwtSecurityTokenHandler();

            JwtSecurityToken? jwtToken;
            try
            {
                if (!handler.CanReadToken(token))
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                jwtToken = handler.ReadJwtToken(token);
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = new List<Claim>();

            foreach (var claim in jwtToken.Claims)
            {
                if (claim.Type == JwtRegisteredClaimNames.Sub || claim.Type == "sub")
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                else if (claim.Type == "role")
                    claims.Add(new Claim(ClaimTypes.Role, claim.Value));
                else
                    claims.Add(claim);
            }

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);

        }

        public void NotifyUserAuthentication(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                return;

            var jwtToken = handler.ReadJwtToken(token);
            var claims = new List<Claim>();

            foreach (var claim in jwtToken.Claims)
            {
                if (claim.Type == JwtRegisteredClaimNames.Sub || claim.Type == "sub")
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                else if (claim.Type == "role")
                    claims.Add(new Claim(ClaimTypes.Role, claim.Value));
                else
                    claims.Add(claim);
            }

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}
