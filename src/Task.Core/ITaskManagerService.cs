using Shared.Web.Service;
using System;
using System.Threading.Tasks;
using TaskManager.Core.DTO;

namespace TaskManager.Core
{
    public interface ITaskManagerService
    {
        Task<ServiceResponse<bool>> CreateAsync(TaskModel item);
        Task<ServiceResponse<bool>> UpdateAsync(TaskModel item);
        Task<ServiceResponse<PageResults<TaskModel>>> GetByOwnerAsync(Guid id, PageConfig settings);
        Task<ServiceResponse<PageResults<TaskModel>>> GetByStatusAsync(bool completed, PageConfig settings);
        Task<ServiceResponse<TaskModel>> GetByIdAsync(Guid id);
        Task<ServiceResponse<TaskModel>> CompleteByIdAsync(Guid id);
    }
}
