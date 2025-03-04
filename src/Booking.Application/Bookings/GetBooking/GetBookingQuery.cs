using Booking.Application.Abstractions.Messaging;

namespace Booking.Application.Bookings.GetBooking
{
    public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;
}
