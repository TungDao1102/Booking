using Booking.Application.Abstractions.Repositories;
using Booking.Domain.Reviews;
using Booking.Infrastructure.Data;

namespace Booking.Infrastructure.Repositories
{

    internal sealed class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
