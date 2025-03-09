using Booking.Application.Abstractions.Repositories;
using Booking.Domain.Apartments;
using Booking.Domain.Bookings;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories
{
    internal sealed class BookingRepository(AppDbContext context) : Repository<Domain.Bookings.Booking>(context), IBookingRepository
    {
        private static readonly BookingStatus[] ActiveBookingStatuses =
    {
        BookingStatus.Reserved,
        BookingStatus.Confirmed,
        BookingStatus.Completed
    };
        public async Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<Domain.Bookings.Booking>().AnyAsync(booking =>
                   booking.ApartmentId == apartment.Id &&
                   booking.Duration.Start <= duration.End &&
                   booking.Duration.End >= duration.Start &&
                   ActiveBookingStatuses.Contains(booking.Status), cancellationToken);
        }
    }
}
