using System;

namespace TaskSample.Services.Features.Tasks.Models
{
    public class TaskCreateModel
    {
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
    }
}
