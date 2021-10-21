using System;

namespace TaskManager.Data.Model
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public DateTime Updated { get; set; }
        public string Description { get; set; }
        public Guid Owner { get; set; }
        public JobStatus State { get; set; }
        public string Notes { get; set; }
    }
}
