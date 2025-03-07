using Booking.Application.Abstractions.Messaging;

namespace Booking.Application.Users.LoginUser
{
    public sealed record LoginUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;
}
