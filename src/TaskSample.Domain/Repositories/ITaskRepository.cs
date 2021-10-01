using System;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Domain.Entities;

namespace TaskSample.Domain.Repositories
{
    public interface ITaskRepository : IGenericRepository<DemoTask>
    {
        Task<DataResult<DemoTask>> GetByOwnerIdAsync(Guid ownerId, DataPaging dataPaging, CancellationToken token = default);
        Task<DataResult<DemoTask>> GetByStatusAsync(bool isComplete, DataPaging dataPaging, CancellationToken token = default);
    }
}
