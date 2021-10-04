using TaskSample.Services.Common.Paging;

namespace TaskSample.Services.Features.Tasks.Models
{
    public class TaskStatusQuery: PagingModel
    {
        public bool IsComplete { get; set; }
    }
}
