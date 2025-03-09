using Booking.Domain.Abstractions;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories
{
    internal abstract class Repository<T>(AppDbContext context) where T : BaseEntity
    {
        protected readonly AppDbContext DbContext = context;
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<T>().FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public virtual void Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }
    }
}
