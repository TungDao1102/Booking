using Booking.Domain.Commons;

namespace Booking.Domain.Users
{
    public static class UserErrors
    {
        public static readonly Error NotFound = new(
            "User.Found",
            "The user with the specified identifier was not found");

        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials were invalid");
    }
}
