using Booking.Domain.Abstractions;

namespace Booking.Domain.UnitTests.Commons
{
    public abstract class BaseTest
    {
        public static T AssertDomainEventWasPublished<T>(BaseEntity entity) where T : IDomainEvent
        {
            T? domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault();

            return domainEvent == null ? throw new Exception($"{typeof(T).Name} was not published") : domainEvent;
        }
    }
}
