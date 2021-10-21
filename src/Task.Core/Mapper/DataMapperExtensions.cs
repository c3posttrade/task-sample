using System;
using TaskManager.Core.DTO;
using TaskManager.Data.Model;

namespace TaskManager.Core.Mapper
{
    public static class DataMapperExtensions
    {
        public static bool IsCompleted(this Job item)
        {
            return item.Status == JobStatus.Completed ||
                   item.Status == JobStatus.Cancelled;
        }

        public static TaskModel ToModel(this Job item)
        {
            return new TaskModel
            {
                Id = item.TaskId,
                Description = item.Description,
                Created = item.Created,
                Updated = item.Updated,
                Owner = item.Owner,
                Completed = item.IsCompleted()
            };
        }

        public static Job ToJob(this TaskModel item)
        {
            return new Job
            {
                Id = Guid.NewGuid(),
                TaskId = item.Id,
                Created = item.Created,
                Description = item.Description,
                Status = JobStatus.Created,
                Owner = item.Owner
            };
        }

        public static AuditLog ToAuditLog(this TaskModel item)
        {
            return new AuditLog
            {
                TaskId = item.Id,
                Updated = DateTime.UtcNow,
                Description = item.Description,
                State = JobStatus.Created,
                Owner = item.Owner
            };
        }

        public static void UpdateFromModel(this Job item, TaskModel model)
        {
            item.Owner = model.Owner;
            item.Description = model.Description;
            item.TaskId = model.Id;
            item.Updated = DateTime.UtcNow;
            if (model.Completed && !item.IsCompleted())
            {
                item.Status = JobStatus.Completed;
            }
            else if (item.IsCompleted() && !model.Completed)
            {
                item.Status = JobStatus.Reopened;
            }
        }
    }
}
