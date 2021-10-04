using System;

namespace TaskSample.Domain.Entities
{
    public class AuditEntityBase<T>
    {
        public T Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}
