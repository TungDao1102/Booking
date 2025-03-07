using Booking.Domain.Commons;

namespace Booking.Application.Abstractions.Authentications
{
    public interface IJwtService
    {
        Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}
