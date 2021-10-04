using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Domain;
using TaskSample.Domain.Entities;
using TaskSample.Domain.Repositories;

namespace TaskSample.Infrastructure.Persistence.EF.RepositoriesImplementation
{
    public class TaskRepository : GenericRepository<DemoTask>, ITaskRepository
    {
        private readonly SampleTaskDbContext _context;
        public TaskRepository(SampleTaskDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DataResult<DemoTask>> GetByOwnerIdAsync(Guid ownerId, DataPaging dataPaging, CancellationToken token = default)
        {
            var query = _context.Tasks.Include(x => x.Owner).Where(y => y.OwnerId == ownerId).Select(x => x).AsQueryable();
            return await ExecuteQuery(query, dataPaging, token);
        }

        private static async Task<DataResult<DemoTask>> ExecuteQuery(IQueryable<DemoTask> query, DataPaging dataPaging, CancellationToken token)
        {
            return new DataResult<DemoTask>
            {
                TotalRecords = await query.CountAsync(token),
                FilteredRecords = await query.Skip(dataPaging.Skip).Take(dataPaging.Take).ToListAsync(token)
            };
        }

        public async Task<DataResult<DemoTask>> GetByStatusAsync(bool isComplete, DataPaging dataPaging, CancellationToken token = default)
        {
            var query = _context.Tasks.Include(x => x.Owner).Where(y => y.IsCompleted == isComplete).Select(x => x).AsQueryable();
            return await ExecuteQuery(query, dataPaging, token);
        }
    }
}
