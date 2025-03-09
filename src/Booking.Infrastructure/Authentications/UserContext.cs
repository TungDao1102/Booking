using Booking.Application.Abstractions.Authentications;
using Microsoft.AspNetCore.Http;

namespace Booking.Infrastructure.Authentications
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public Guid UserId => httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ?? throw new ApplicationException("User context is unavailable");

        public string IdentityId => httpContextAccessor
            .HttpContext?
            .User
            .GetIdentityId() ?? throw new ApplicationException("User context is unavailable");
    }
}
