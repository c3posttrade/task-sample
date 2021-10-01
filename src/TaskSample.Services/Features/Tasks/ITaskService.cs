using System;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Services.Common.Paging;
using TaskSample.Services.Features.Tasks.Models;

namespace TaskSample.Services.Features.Tasks
{
    public interface ITaskService
    {
        Task<TaskDetailViewModel> CreateAsync(TaskCreateModel taskModel, CancellationToken token = default);
        Task<TaskDetailViewModel> GetByIdAsync(Guid taskId, CancellationToken token = default);
        Task MarkCompleteAsync(Guid taskId, CancellationToken token = default);
        Task<PagedResult<TaskDetailViewModel>> GetByOwnerAsync(Guid ownerId, PagingModel paging, CancellationToken token = default);
        Task<PagedResult<TaskDetailViewModel>> GetByStatusAsync(bool isComplete, PagingModel paging, CancellationToken token = default);
    }
}
