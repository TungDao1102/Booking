using Microsoft.AspNetCore.Authorization;

namespace Booking.Infrastructure.Authorizations
{
    public sealed class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
    {

    }
}
