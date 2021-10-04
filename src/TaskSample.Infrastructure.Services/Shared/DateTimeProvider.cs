using System;
using TaskSample.Shared;

namespace TaskSample.Infrastructure.Services.Shared
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset DateTimeNow => DateTimeOffset.Now;
    }
}
