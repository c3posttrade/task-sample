using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Domain;
using TaskSample.Domain.Entities;
using TaskSample.Services.Common.Paging;
using TaskSample.Services.Exceptions;
using TaskSample.Services.Features.Tasks;
using TaskSample.Services.Features.Tasks.Models;
using TaskSample.Shared;

namespace TaskSample.Infrastructure.Services.Features
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region PrivateMethods
        private async Task CheckOwnerExistence(Guid ownerId, CancellationToken token)
        {
            var isOwnerExists = await _unitOfWork.OwnerRepository.FindByIdAsync(ownerId, token);
            if (isOwnerExists is null) throw new NotFoundException($"Owner with Id: {ownerId} doesn't exist");
        }
        private PagedResult<TaskDetailViewModel> PageResult(PagingModel paging, DataResult<DemoTask> tasks)
        {
            var filteredRecords = _mapper.Map<List<TaskDetailViewModel>>(tasks.FilteredRecords);
            return new PagedResult<TaskDetailViewModel>(filteredRecords, tasks.TotalRecords, paging);
        }
        #endregion

        public async Task<TaskDetailViewModel> CreateAsync(TaskCreateModel taskModel, CancellationToken token = default)
        {
            if (taskModel is null) throw new ArgumentNullException(nameof(taskModel));

            await CheckOwnerExistence(taskModel.OwnerId, token);

            var demoTask = _mapper.Map<DemoTask>(taskModel);
            demoTask.Created = _dateTimeProvider.DateTimeNow;

            var newTask = await _unitOfWork.TaskRepository.AddAsync(demoTask, token);
            await _unitOfWork.SaveChangesAsync(token);
            return _mapper.Map<TaskDetailViewModel>(newTask);
        }

        public async Task<TaskDetailViewModel> GetByIdAsync(Guid taskId, CancellationToken token = default)
        {
            var task = await _unitOfWork.TaskRepository.FindByIdAsync(taskId, token);
            if (task is null) throw new NotFoundException($"Task with Id: {taskId} not found");
            return _mapper.Map<TaskDetailViewModel>(task);
        }

        public async Task<PagedResult<TaskDetailViewModel>> GetByOwnerAsync(Guid ownerId, PagingModel paging, CancellationToken token = default)
        {
            var tasks = await _unitOfWork.TaskRepository.GetByOwnerIdAsync(ownerId, _mapper.Map<DataPaging>(paging), token);
            return PageResult(paging, tasks);
        }

        public async Task<PagedResult<TaskDetailViewModel>> GetByStatusAsync(bool isComplete, PagingModel paging, CancellationToken token = default)
        {
            var tasks = await _unitOfWork.TaskRepository.GetByStatusAsync(isComplete, _mapper.Map<DataPaging>(paging), token);
            return PageResult(paging, tasks);
        }

        public async Task MarkCompleteAsync(Guid taskId, CancellationToken token = default)
        {
            var originalTask = await _unitOfWork.TaskRepository.FindByIdAsync(taskId, token);
            if (originalTask is null) throw new NotFoundException($"Task with Id: {taskId} not found");

            originalTask.IsCompleted = true;
            await _unitOfWork.TaskRepository.UpdateAsync(originalTask, token);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
