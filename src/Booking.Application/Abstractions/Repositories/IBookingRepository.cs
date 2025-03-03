using Booking.Domain.Apartments;
using Booking.Domain.Bookings;

namespace Booking.Application.Abstractions.Repositories
{
    public interface IBookingRepository
    {
        Task<Domain.Bookings.Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsOverlappingAsync(
            Apartment apartment,
            DateRange duration,
            CancellationToken cancellationToken = default);

        void Add(Domain.Bookings.Booking booking);
    }
}
