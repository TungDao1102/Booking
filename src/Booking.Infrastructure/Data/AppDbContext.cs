using Booking.Application.Abstractions.Repositories;
using Booking.Application.Exceptions;
using Booking.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Data
{
    public sealed class AppDbContext(DbContextOptions options, IPublisher publisher) : DbContext(options), IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await PublishEventsAsync();

                int result = await base.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency exception occurred.", ex);
            }
        }

        private async Task PublishEventsAsync()
        {
            var domainEvents = ChangeTracker.Entries<BaseEntity>()
                    .Select(x => x.Entity)
                    .SelectMany(x =>
                    {
                        var domainEvent = x.GetDomainEvents();
                        x.ClearDomainEvents();
                        return domainEvent;
                    }).ToList();

            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent);
            }
        }
    }
}
