using System;

namespace TaskSample.Domain.Entities
{
    public class Owner : AuditEntityBase<Guid>
    {
        public string Name { get; set; }
    }
}
