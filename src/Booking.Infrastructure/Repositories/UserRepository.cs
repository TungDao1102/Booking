using Booking.Application.Abstractions.Repositories;
using Booking.Domain.Users;
using Booking.Infrastructure.Data;

namespace Booking.Infrastructure.Repositories
{
    internal class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
    {
    }
}
