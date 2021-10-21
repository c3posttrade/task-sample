using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Web.Service;
using System;
using System.Threading.Tasks;
using TaskManager.Core;
using TaskManager.Core.DTO;

namespace TaskManager.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        const int DEFAULT_PAGE_SIZE = 25;
        private readonly ILogger<TasksController> _logger;
        private readonly ITaskManagerService _service;

        public TasksController(ILogger<TasksController> logger, ITaskManagerService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// As an api user, I need to be able to create a task with a given description and owner so that I can store information about a task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(TaskModel model)
        {
            return await CallHandler.Process(() => _service.CreateAsync(model), _logger, this);
        }

        /// <summary>
        /// As an api user, I need to be able to mark a task as completed so that I am able to specify when a task is done 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Complete(Guid id)
        {
            return await CallHandler.Process(() => _service.CompleteByIdAsync(id), _logger, this);
        }

        /// <summary>
        /// As an api user, I need to be able to retrieve tasks by owner so that I can identify tasks created by a specific owner
        /// As an api user, I need to be able to page tasks returned from the api so that I can process large result sets
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("byOwner")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PageResults<TaskModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByOwner(Guid owner, int page, int pageSize)
        {
            var pageConfig = FromParams(page, pageSize);
            return await CallHandler.Process(() => _service.GetByOwnerAsync(owner, pageConfig), _logger, this);
        }

        /// <summary>
        /// As an api user, I need to be able to retrieve tasks by the completed field so that I can identify tasks that are active or in-active
        /// </summary>
        /// <param name="completed"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("byStatus")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PageResults<TaskModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByStatus(bool completed, int page, int pageSize)
        {
            var pageConfig = FromParams(page, pageSize);
            return await CallHandler.Process(() => _service.GetByStatusAsync(completed, pageConfig), _logger, this);
        }

        /// <summary>
        /// As an api user, I need to be able to retrieve a task by its id so that I can look up information for a specific task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await CallHandler.Process(() => _service.GetByIdAsync(id), _logger, this);
        }

        private static PageConfig FromParams(int page, int pageSize)
        {
            return new PageConfig { Page = page < 0 ? 0 : page, PageSize = pageSize <= 0 ? DEFAULT_PAGE_SIZE : pageSize };
        }
    }
}
