using Booking.Application.Abstractions.Repositories;
using Booking.Domain.Apartments;
using Booking.Infrastructure.Data;

namespace Booking.Infrastructure.Repositories
{
    internal sealed class ApartmentRepository(AppDbContext context) : Repository<Apartment>(context), IApartmentRepository
    {
    }
}
