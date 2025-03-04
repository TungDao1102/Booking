using Booking.Domain.Abstractions;
using MediatR;

namespace Booking.Application.Events
{
    public class DomainEventAdapter<T>(T domainEvent) : INotification where T : IDomainEvent
    {
        public T DomainEvent { get; } = domainEvent;
    }
}
