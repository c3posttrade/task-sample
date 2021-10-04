using System;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Domain;

namespace TaskSample.Infrastructure.Persistence.EF
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly SampleTaskDbContext _context;
        public GenericRepository(SampleTaskDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken token = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, token).ConfigureAwait(false);
            return entity;
        }

        public async Task<TEntity> FindByIdAsync(object id, CancellationToken token = default)
        {
            return await _context.Set<TEntity>().FindAsync(new object[] { id }, token).ConfigureAwait(false);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken token = default)
        {
            await Task.Run(() =>
            {
                _context.Update(entity);
            }, token);
        }
    }
}
