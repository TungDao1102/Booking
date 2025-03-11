using Booking.Application.Abstractions.Clocks;
using Booking.Application.Abstractions.Messaging;
using Booking.Application.Abstractions.Repositories;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using Booking.Domain.Reviews;

namespace Booking.Application.Reviews
{
    public class AddReviewCommandHandler(
        IReviewRepository reviewRepository,
        IBookingRepository bookingRepository,
        IDateTimeProvider timeProvider,
        IUnitOfWork unitOfWork) : ICommandHandler<AddReviewCommand>
    {
        public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

            if (booking is null)
            {
                return Result.Failure(BookingErrors.NotFound);
            }

            Result<Rating> ratingResult = Rating.Create(request.Rating);

            if (ratingResult.IsFailure)
            {
                return Result.Failure(ratingResult.Error);
            }

            Result<Review> reviewResult = Review.Create(
                booking,
                ratingResult.Value,
                new Comment(request.Comment),
                timeProvider.UtcNow);

            if (reviewResult.IsFailure)
            {
                return Result.Failure(reviewResult.Error);
            }

            reviewRepository.Add(reviewResult.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
