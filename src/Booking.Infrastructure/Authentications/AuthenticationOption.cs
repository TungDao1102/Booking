namespace Booking.Infrastructure.Authentications
{
    public sealed class AuthenticationOption
    {
        public string Audience { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string MetadataUrl { get; init; } = string.Empty;
        public bool RequireHttpsMetadata { get; init; }
    }
}
