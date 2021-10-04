using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Services.Common.Paging;
using TaskSample.Services.Features.Tasks;
using TaskSample.Services.Features.Tasks.Models;

namespace TaskSample.Api.Controllers
{
    [ApiController]
    [Route("/api/tasks")]
    [Produces("application/json")]
    public class TasksController : ControllerApiBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TaskDetailViewModel>> CreateAsync([FromBody] TaskCreateModel task, CancellationToken cancellationToken = default)
        {
            var newTask = await _taskService.CreateAsync(task, cancellationToken);
            return Created(nameof(GetByIdAsync), new { id = newTask.Id });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDetailViewModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await _taskService.GetByIdAsync(id, cancellationToken);
            if (task is null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<TaskDetailViewModel>>> GetByStatusAsync([FromQuery] TaskStatusQuery statusQuery, CancellationToken cancellationToken = default)
        {
            var tasks = await _taskService.GetByStatusAsync(statusQuery.IsComplete, new PagingModel { PageNumber = statusQuery.PageNumber, PageSize = statusQuery.PageSize }, cancellationToken);
            return Ok(tasks);
        }

        [HttpGet]
        [Route("/api/owners/{ownerId}/tasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<TaskDetailViewModel>>> GetByOwnerAsync(Guid ownerId, [FromQuery] PagingModel paging, CancellationToken cancellationToken = default)
        {
            var tasksByOwner = await _taskService.GetByOwnerAsync(ownerId, paging, cancellationToken);
            return Ok(tasksByOwner);
        }

        [HttpPut("{id}/mark-complete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> MarkTaskCompleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError("id", "Task id is required");
                return BadRequest();
            }
            await _taskService.MarkCompleteAsync(id, cancellationToken);
            return new NoContentResult();
        }
    }
}
