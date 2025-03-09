using Booking.Infrastructure.Authentications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure.Authorizations
{
    public sealed class PermissionAuthorizationHandler(IServiceProvider serviceProvider) : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.Identity is not { IsAuthenticated: true })
            {
                return;
            }

            using IServiceScope scope = serviceProvider.CreateScope();
            var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

            var identityId = context.User.GetIdentityId();
            HashSet<string> permissions = await authorizationService.GetUserPermissionsAsync(identityId);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
