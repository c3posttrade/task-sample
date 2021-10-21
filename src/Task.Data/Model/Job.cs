using System;

namespace TaskManager.Data.Model
{
    public enum JobStatus
    {
        Created, InProgress, Blocked, Completed, Cancelled, Reopened
    }

    public class Job
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public Guid Owner { get; set; }
        public DateTime? Updated { get; set; }
        public JobStatus Status { get; set; }
    }
}
