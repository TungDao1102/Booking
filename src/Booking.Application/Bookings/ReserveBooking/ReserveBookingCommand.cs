using Booking.Application.Abstractions.Messaging;

namespace Booking.Application.Bookings.ReserveBooking
{
    public record ReserveBookingCommand(Guid ApartmentId, Guid UserId, DateOnly DateStart, DateOnly DateEnd) : ICommand<Guid>;
}
