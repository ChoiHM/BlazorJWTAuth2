using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorJWTAuth2.Auth
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
            var tokenString = await _localStorage.ReadEncryptedItemAsync<string>("jwt");

            if (string.IsNullOrEmpty(tokenString) || IsTokenValid(tokenString) == false)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var jwtToken = new JwtSecurityToken(tokenString);

            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            // Admin권한을 갖고있는지 확인
            // if(user.IsInRole("Admin"))
            // { 
            // }
            return new AuthenticationState(user);
        }

        public event Action OnChange;
        public void NotifyUserAuthentication(string tokenString)
        {
            var jwtToken = new JwtSecurityToken(tokenString);
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
            OnChange?.Invoke();
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
            OnChange?.Invoke();
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
                return false;

            var jwtToken = tokenHandler.ReadJwtToken(token);
            if (jwtToken != null && jwtToken.Payload.Exp != null)
            {
                var expiration = jwtToken.Payload.Exp;
                var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expiration.Value).DateTime;
                return expirationDate > DateTime.UtcNow;
            }
            else
            {
                return false;
            }
        }
    }
}
