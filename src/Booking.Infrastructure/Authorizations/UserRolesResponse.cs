using Booking.Domain.Users.Authorizations;

namespace Booking.Infrastructure.Authorizations
{
    public sealed class UserRolesResponse
    {
        public Guid Id { get; init; }
        public List<Role> Roles { get; init; } = [];
    }
}
