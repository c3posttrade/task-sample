using System.Threading.Tasks;
using System.Threading;
using TaskSample.Domain.Repositories;

namespace TaskSample.Domain
{
    public interface IUnitOfWork
    {
        //Begin Transaction
        //Commit Transaction
        //Rollback Transaction
        ITaskRepository TaskRepository { get; }
        IOwnerRepository OwnerRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}
