using Booking.Domain.Users;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Authorizations
{
    internal sealed class AuthorizationService(AppDbContext context)
    {
        public async Task<UserRolesResponse> GetUserRolesAsync(string identityId)
        {
            return await context.Set<User>().Where(x => x.IdentityId == identityId)
                .Select(x => new UserRolesResponse
                {
                    Id = x.Id,
                    Roles = x.Roles.ToList()
                }).FirstAsync();
        }
    }
}
