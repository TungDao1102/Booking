using Booking.Application.Abstractions.Messaging;

namespace Booking.Application.Reviews
{
    public sealed record AddReviewCommand(Guid BookingId, int Rating, string Comment) : ICommand;
}
