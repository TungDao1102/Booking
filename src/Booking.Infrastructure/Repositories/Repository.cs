using Booking.Domain.Abstractions;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories
{
    internal abstract class Repository<T>(AppDbContext context) where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet = context.Set<T>();
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }
    }
}
