using Microsoft.AspNetCore.Authorization;

namespace Booking.Infrastructure.Authorizations
{
    public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
