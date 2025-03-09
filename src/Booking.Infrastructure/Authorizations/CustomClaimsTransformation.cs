using Booking.Infrastructure.Authentications;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Booking.Infrastructure.Authorizations
{
    /// <summary>
    /// Custom claims before authorization
    /// </summary>
    /// <param name="serviceProvider"></param>
    internal sealed class CustomClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.HasClaim(x => x.Type == ClaimTypes.Role)
                && principal.HasClaim(x => x.Type == JwtRegisteredClaimNames.Sub))
            {
                return principal;
            }

            using var scope = serviceProvider.CreateScope();
            var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

            string identityId = principal.GetIdentityId();
            var userRoles = await authorizationService.GetUserRolesAsync(identityId);
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.Id.ToString()));

            foreach (var role in userRoles.Roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }
            principal.AddIdentity(claimsIdentity);
            return principal;
        }
    }
}
