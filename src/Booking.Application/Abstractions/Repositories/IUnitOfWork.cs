namespace Booking.Application.Abstractions.Repositories
{
    public interface IUnitOfWork
    {
        ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
