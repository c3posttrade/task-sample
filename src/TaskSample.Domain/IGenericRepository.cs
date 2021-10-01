using System.Threading;
using System.Threading.Tasks;

namespace TaskSample.Domain
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken token = default);
        Task<TEntity> FindByIdAsync(object id, CancellationToken token = default);
        Task UpdateAsync(TEntity entity, CancellationToken token = default);
    }
}
