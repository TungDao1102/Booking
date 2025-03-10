using Booking.Application.Abstractions.Caching;
using Booking.Domain.Users;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Authorizations
{
    internal sealed class AuthorizationService(AppDbContext context, ICacheService cacheService)
    {
        public async Task<UserRolesResponse> GetUserRolesAsync(string identityId)
        {
            var cacheKey = $"auth:roles-{identityId}";
            var cachedRoles = await cacheService.GetAsync<UserRolesResponse>(cacheKey);

            if (cachedRoles is not null)
            {
                return cachedRoles;
            }

            var roles = await context.Set<User>().Where(x => x.IdentityId == identityId)
                .Select(x => new UserRolesResponse
                {
                    Id = x.Id,
                    Roles = x.Roles.ToList()
                }).FirstAsync();

            await cacheService.SetAsync(cacheKey, roles);
            return roles;
        }

        public async Task<HashSet<string>> GetUserPermissionsAsync(string identityId)
        {
            var cacheKey = $"auth:permissions-{identityId}";

            var cachedPermissions = await cacheService.GetAsync<HashSet<string>>(cacheKey);

            if (cachedPermissions is not null)
            {
                return cachedPermissions;
            }

            var permissions = (await context.Set<User>()
                .Where(x => x.IdentityId == identityId)
                .SelectMany(x => x.Roles.Select(y => y.Permissions))
                .FirstAsync())
                .Select(x => x.Name)
                .ToHashSet();

            await cacheService.SetAsync(cacheKey, permissions);
            return permissions;
        }
    }
}
