using System;
using TaskSample.Services.Common.Models;

namespace TaskSample.Services.Features.Tasks.Models
{
    public class TaskDetailViewModel : AuditModelBase<Guid>
    {
        public string Description { get; set; }
        public bool Completed { get; set; }
        public string OwnerName { get; set; }
        public Guid OwnerId { get; set; }
    }
}
