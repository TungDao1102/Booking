using Booking.Application.Abstractions.Repositories;
using Booking.Domain.Users;
using Booking.Infrastructure.Data;

namespace Booking.Infrastructure.Repositories
{
    internal class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
    {
        public override void Add(User user)
        {
            foreach (var role in user.Roles)
            {
                DbContext.Attach(role);
            }
            DbContext.Add(user);
        }
    }
}
