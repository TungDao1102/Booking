using Booking.Application.Abstractions.Clocks;
using Booking.Application.Abstractions.Repositories;
using Booking.Application.Exceptions;
using Booking.Domain.Abstractions;
using Booking.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Booking.Infrastructure.Data
{
    public sealed class AppDbContext(DbContextOptions options, IDateTimeProvider timeProvider) : DbContext(options), IUnitOfWork
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await AddDomainEventsAsOutboxMessagesAsync();

                int result = await base.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency exception occurred.", ex);
            }
        }

        private async Task AddDomainEventsAsOutboxMessagesAsync()
        {
            var outboxMessages = ChangeTracker.Entries<BaseEntity>()
                    .Select(x => x.Entity)
                    .SelectMany(x =>
                    {
                        var domainEvent = x.GetDomainEvents();
                        x.ClearDomainEvents();
                        return domainEvent;
                    })
                    .Select(domainEvent => new OutboxMessage(
                        Guid.NewGuid(),
                        timeProvider.UtcNow,
                        domainEvent.GetType().Name,
                        JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
                    .ToList();

            await AddRangeAsync(outboxMessages);
        }
    }
}
