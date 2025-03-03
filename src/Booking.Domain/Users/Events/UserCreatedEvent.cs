using Booking.Domain.Abstractions;

namespace Booking.Domain.Users.Events
{
    public sealed record UserCreatedEvent(Guid UserId) : IDomainEvent;
}
