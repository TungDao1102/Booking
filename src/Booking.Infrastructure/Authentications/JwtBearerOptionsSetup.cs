using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Booking.Infrastructure.Authentications
{
    public sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AuthenticationOption _authenticationOption;
        public JwtBearerOptionsSetup(IOptions<AuthenticationOption> authenticationOption)
        {
            _authenticationOption = authenticationOption.Value;
        }
        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Audience = _authenticationOption.Audience;
            options.MetadataAddress = _authenticationOption.MetadataUrl;
            options.RequireHttpsMetadata = _authenticationOption.RequireHttpsMetadata;
            options.TokenValidationParameters.ValidIssuer = _authenticationOption.Issuer;
        }
    }
}
