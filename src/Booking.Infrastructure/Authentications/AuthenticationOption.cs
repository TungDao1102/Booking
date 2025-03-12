namespace Booking.Infrastructure.Authentications
{
    public sealed class AuthenticationOption
    {
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string MetadataUrl { get; set; } = string.Empty;
        public bool RequireHttpsMetadata { get; set; }
    }
}
