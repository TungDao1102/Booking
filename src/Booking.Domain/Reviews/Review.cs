using Booking.Domain.Abstractions;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using Booking.Domain.Reviews.Events;

namespace Booking.Domain.Reviews
{
    public class Review : BaseEntity
    {
        public Review()
        {

        }

        private Review(
            Guid id,
            Guid apartmentId,
            Guid bookingId,
            Guid userId,
            Rating rating,
            Comment comment,
            DateTime createdOn) : base(id)
        {
            ApartmentId = apartmentId;
            BookingId = bookingId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
            CreatedOn = createdOn;
        }

        public Guid ApartmentId { get; private set; }

        public Guid BookingId { get; private set; }

        public Guid UserId { get; private set; }

        public Rating Rating { get; private set; }

        public Comment Comment { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public static Result<Review> Create(Bookings.Booking booking, Rating rating, Comment comment, DateTime createdOn)
        {
            if (booking.Status != BookingStatus.Completed)
            {
                return Result.Failure<Review>(ReviewErrors.NotEligible);
            }

            var review = new Review(
                Guid.NewGuid(),
                booking.ApartmentId,
                booking.Id,
                booking.UserId,
                rating,
                comment,
                createdOn);

            review.AddDomainEvent(new ReviewCreatedDomainEvent(review.Id));

            return review;
        }
    }
}
