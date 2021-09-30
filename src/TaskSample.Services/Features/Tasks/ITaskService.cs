using System;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Services.Common.Paging;
using TaskSample.Services.Features.Tasks.Models;

namespace TaskSample.Services.Features.Tasks
{
    public interface ITaskService
    {
        Task<TaskDetailViewModel> CreateAsync(TaskCreateModel task, CancellationToken token = default);
        Task<TaskDetailViewModel> GetByIdAsync(Guid taskId, CancellationToken token = default);
        Task MarkCompleteAsync(Guid taskId, CancellationToken token = default);
        Task<PagedResult<TaskDetailViewModel>> GetByOwner(Guid ownerId, PagingModel paging, CancellationToken token = default);
        Task<PagedResult<TaskDetailViewModel>> GetByStatus(bool isComplete, PagingModel paging, CancellationToken token = default);
    }
}
