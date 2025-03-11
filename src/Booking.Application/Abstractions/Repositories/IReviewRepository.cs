using Booking.Domain.Reviews;

namespace Booking.Application.Abstractions.Repositories
{
    public interface IReviewRepository
    {
        void Add(Review review);
    }

}
