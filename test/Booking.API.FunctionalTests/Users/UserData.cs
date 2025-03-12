using Booking.API.Controllers.Users;

namespace Booking.API.FunctionalTests.Users
{
    internal static class UserData
    {
        public static RegisterUserRequest RegisterTestUserRequest = new("test@test.com", "test", "test", "12345");
    }
}
