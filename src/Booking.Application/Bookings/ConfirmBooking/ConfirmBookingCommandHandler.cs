using Booking.Application.Abstractions.Clocks;
using Booking.Application.Abstractions.Messaging;
using Booking.Application.Abstractions.Repositories;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;

namespace Booking.Application.Bookings.ConfirmBooking
{
    public class ConfirmBookingCommandHandler(
        IBookingRepository bookingRepository,
        IDateTimeProvider timeProvider,
        IUnitOfWork unitOfWork) : ICommandHandler<ConfirmBookingCommand>
    {
        public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
            if (booking is null)
            {
                return Result.Failure(BookingErrors.NotFound);
            }

            Result result = booking.Confirm(timeProvider.UtcNow);
            if (result.IsFailure)
            {
                return result;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
