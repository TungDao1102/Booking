using System.Text.Json.Serialization;

namespace Booking.Infrastructure.Authentications.Models
{
    internal sealed class AuthorizationToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;
    }
}
