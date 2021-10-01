using System;

namespace TaskSample.Shared
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset DateTimeNow { get; }
    }
}
