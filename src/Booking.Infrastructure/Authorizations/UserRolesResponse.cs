using Booking.Domain.Users;

namespace Booking.Infrastructure.Authorizations
{
    public sealed class UserRolesResponse
    {
        public Guid Id { get; init; }
        public List<Role> Roles { get; init; } = [];
    }
}
