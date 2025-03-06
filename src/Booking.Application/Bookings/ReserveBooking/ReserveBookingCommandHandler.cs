using Booking.Application.Abstractions.Clocks;
using Booking.Application.Abstractions.Messaging;
using Booking.Application.Abstractions.Repositories;
using Booking.Application.Exceptions;
using Booking.Domain.Apartments;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using Booking.Domain.Users;

namespace Booking.Application.Bookings.ReserveBooking
{
    internal sealed class ReserveBookingCommandHandler(
        IUserRepository userRepository,
        IApartmentRepository apartmentRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        PricingService pricingService,
        IDateTimeProvider dateTimeProvider) : ICommandHandler<ReserveBookingCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            var apartment = await apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);
            if (apartment is null)
            {
                return Result.Failure<Guid>(ApartmentErrors.NotFound);
            }

            var duration = DateRange.Create(request.DateStart, request.DateEnd);

            if (await bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }
            try
            {
                var booking = Domain.Bookings.Booking.Reserve(apartment,
                                            request.UserId,
                                            duration,
                                            dateTimeProvider.UtcNow,
                                            pricingService);
                bookingRepository.Add(booking);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                return booking.Id;
            }
            catch (ConcurrencyException)
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }
        }
    }
}
