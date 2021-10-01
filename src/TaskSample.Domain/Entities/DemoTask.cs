using System;

namespace TaskSample.Domain.Entities
{
    public class DemoTask : AuditEntityBase<Guid>
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public Guid OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}
