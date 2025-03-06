namespace Booking.Domain.Abstractions
{
    public abstract class BaseEntity
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        public Guid Id { get; init; }
        protected BaseEntity()
        {
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents;
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
