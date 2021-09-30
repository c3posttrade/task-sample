using System;

namespace TaskSample.Services.Common.Models
{
    public class AuditModelBase<MId> : ModelBase<MId>
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}
