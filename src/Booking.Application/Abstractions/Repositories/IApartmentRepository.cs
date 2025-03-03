using Booking.Domain.Apartments;

namespace Booking.Application.Abstractions.Repositories
{
    public interface IApartmentRepository
    {
        Task<Apartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
