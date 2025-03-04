using Booking.Application.Abstractions.Email;
using Booking.Application.Abstractions.Repositories;
using Booking.Application.Events;
using Booking.Domain.Bookings.Events;
using MediatR;

namespace Booking.Application.Bookings.ReserveBooking
{
    internal sealed class ReserveBookingEventHandler(
        IBookingRepository bookingRepository,
        IUserRepository userRepository,
        IEmailService emailService) : INotificationHandler<DomainEventAdapter<BookingReservedDomainEvent>>
    {
        public async Task Handle(DomainEventAdapter<BookingReservedDomainEvent> notification, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(notification.DomainEvent.BookingId, cancellationToken);
            if (booking is null)
            {
                return;
            }

            var user = await userRepository.GetByIdAsync(booking.UserId, cancellationToken);
            if (user is null)
            {
                return;
            }

            await emailService.SendAsync(user.Email, "Booking reserved", $"Your booking {booking.Id} has been reserved.");
        }
    }
}
