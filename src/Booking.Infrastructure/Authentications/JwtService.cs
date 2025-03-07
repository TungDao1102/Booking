using System.Net.Http.Json;
using Booking.Application.Abstractions.Authentications;
using Booking.Domain.Commons;
using Booking.Infrastructure.Authentications.Models;
using Microsoft.Extensions.Options;

namespace Booking.Infrastructure.Authentications
{
    public class JwtService : IJwtService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakOptions _keycloakOptions;

        private static readonly Error AuthenticationFailed = new("Keycloak.AuthenticationFailed", "Failed to acquire access token do to authentication failure");

        public JwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
        {
            _httpClient = httpClient;
            _keycloakOptions = keycloakOptions.Value;
        }
        public async Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                var authRequestParameters = new KeyValuePair<string, string>[]
                {
                    new("client_id", _keycloakOptions.AuthClientId),
                    new("client_secret", _keycloakOptions.AuthClientSecret),
                    new("scope", "openid email"),
                    new("grant_type", "password"),
                    new("username", email),
                    new("password", password)
                };

                using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);
                HttpResponseMessage response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);
                response.EnsureSuccessStatusCode();

                AuthorizationToken? authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken);
                return authorizationToken is null ? Result.Failure<string>(AuthenticationFailed) : authorizationToken.AccessToken;
            }
            catch (HttpRequestException)
            {
                return Result.Failure<string>(AuthenticationFailed);
            }
        }
    }
}
