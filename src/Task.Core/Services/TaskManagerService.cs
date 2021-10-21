using Shared.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Core.DTO;
using TaskManager.Core.Mapper;
using TaskManager.Core.Validators;
using TaskManager.Data;
using TaskManager.Data.Model;

namespace TaskManager.Core.Services
{
    public class TaskManagerService : ITaskManagerService
    {
        private readonly IDataContextFactory _factory;

        public TaskManagerService(IDataContextFactory factory)
        {
            _factory = factory;
        }

        public async Task<ServiceResponse<bool>> CreateAsync(TaskModel item)
        {
            if (item.IsValid())
            {
                var context = _factory.Build();
                var existing = context.Jobs.Any(x => x.TaskId == item.Id);
                if (!existing)
                {
                    var data = item.ToJob();
                    context.Jobs.Add(data);
                    var trace = item.ToAuditLog();
                    context.AuditLogs.Add(trace);
                    await context.SaveChangesAsync();
                    return new ServiceResponse<bool>(true);
                }
            }
            return new ServiceResponse<bool> { Status = ResponseStatus.BadRequest };
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(TaskModel item)
        {
            if (item.IsValid())
            {
                var context = _factory.Build();
                var data = context.Jobs.FirstOrDefault(x => x.TaskId == item.Id);
                data.UpdateFromModel(item);
                var trace = item.ToAuditLog();
                context.AuditLogs.Add(trace);
                var updates = await context.SaveChangesAsync();
                return new ServiceResponse<bool>(updates > 0);
            }
            return new ServiceResponse<bool> { Status = ResponseStatus.BadRequest };
        }

        public Task<ServiceResponse<TaskModel>> GetByIdAsync(Guid id)
        {
            var context = _factory.Build();
            // TODO improve this - async
            var model = context.Jobs.Where(x => x.TaskId == id).Select(x => x.ToModel()).FirstOrDefault();
            if (model != null)
            {
                var response = new ServiceResponse<TaskModel>(model);
                return Task.FromResult(response);
            }
            else
            {
                var response = new ServiceResponse<TaskModel> { Status = ResponseStatus.NotFound };
                return Task.FromResult(response);
            }
        }

        public Task<ServiceResponse<PageResults<TaskModel>>> GetByOwnerAsync(Guid id, PageConfig page)
        {
            var context = _factory.Build();
            var query = context.Jobs.Where(x => x.Owner == id);
            var model = GetPage<TaskModel>(query, page);
            var response = new ServiceResponse<PageResults<TaskModel>>(model);
            return Task.FromResult(response);
        }

        public Task<ServiceResponse<PageResults<TaskModel>>> GetByStatusAsync(bool completed, PageConfig page)
        {
            var context = _factory.Build();
            var query = completed ? // avoid LINQ to EF/translation
                context.Jobs.Where(x => x.Status == JobStatus.Completed || x.Status == JobStatus.Cancelled) :
                context.Jobs.Where(x => !(x.Status == JobStatus.Completed || x.Status == JobStatus.Cancelled));
            var model = GetPage<TaskModel>(query, page);
            var response = new ServiceResponse<PageResults<TaskModel>>(model);
            return Task.FromResult(response);
        }

        public async Task<ServiceResponse<TaskModel>> CompleteByIdAsync(Guid id)
        {
            var context = _factory.Build();
            var data = context.Jobs.Where(x => x.TaskId == id).FirstOrDefault();
            if (data != null)
            {
                data.Status = JobStatus.Completed;
                var updates = await context.SaveChangesAsync();
                return new ServiceResponse<TaskModel>(data.ToModel());
            }
            return new ServiceResponse<TaskModel> { Status = ResponseStatus.NotFound };
        }

        protected static PageResults<T> GetPage<T>(IQueryable<Job> query, PageConfig page)
        {
            int total = query.Count();
            if (page != null && page.PageSize > 0)
            {
                var items = query.Skip(page.Page * page.PageSize)
                    .Take(page.PageSize)
                    .Select(x => x.ToModel())
                    .Cast<T>()
                    .ToList();
                return new PageResults<T>
                {
                    CurrentPage = page.Page,
                    TotalItems = total,
                    TotalPages = (int)Math.Ceiling((float)total / page.PageSize),
                    Items = items
                };
            }
            return new PageResults<T>
            {
                TotalItems = total,
                TotalPages = 0,
                CurrentPage = -1,
                Items = new List<T>()
            };
        }
    }
}
