using Booking.Domain.Users;

namespace Booking.Application.Abstractions.Authentications
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken);
    }
}
