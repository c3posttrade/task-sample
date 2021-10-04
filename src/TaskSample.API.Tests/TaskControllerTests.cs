using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Api.Controllers;
using TaskSample.Services.Common.Paging;
using TaskSample.Services.Features.Tasks;
using TaskSample.Services.Features.Tasks.Models;

namespace TaskSample.API.Tests
{
    public class TestControllerTests
    {

        TasksController _sut;
        Mock<ITaskService> _mockTaskService;

        [SetUp]
        public void Setup()
        {
            _mockTaskService = new Mock<ITaskService>();
            _sut = new TasksController(_mockTaskService.Object);
        }

        [Test]
        [AutoData]
        public async Task CreateAsync_WhenValidModelPassed_ReturnsCorrectResult(TaskDetailViewModel taskDetail, CancellationTokenSource tokenSource)
        {
            //Arrange
            TaskCreateModel taskModel = new() { Description = taskDetail.Description, OwnerId = taskDetail.OwnerId };
            _mockTaskService.Setup(x => x.CreateAsync(It.IsAny<TaskCreateModel>(), tokenSource.Token)).ReturnsAsync(taskDetail);

            //Act
            var result = await _sut.CreateAsync(taskModel, tokenSource.Token);

            //Assert
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOf(typeof(CreatedResult), result.Result);
            var routeResult = result.Result as CreatedResult;
            Assert.AreEqual(201, routeResult.StatusCode);
            Assert.IsNotNull(routeResult.Value);
        }

        [Test]
        public async Task GetByIdAsync_WhenTaskNotFound_ReturnsNotFoundResult()
        {
            //Arrange
            TaskDetailViewModel task = null;
            CancellationTokenSource tokenSource = new();
            _mockTaskService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), tokenSource.Token)).ReturnsAsync(task);

            //Act
            var result = await _sut.GetByIdAsync(Guid.NewGuid(), tokenSource.Token);

            //Assert
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOf(typeof(NotFoundResult), result.Result);
            var routeResult = result.Result as NotFoundResult;
            Assert.AreEqual(404, routeResult.StatusCode);
        }

        [Test]
        [AutoData]
        public async Task GetByIdAsync_WhenTaskFound_ReturnsCorrectResult(TaskDetailViewModel task, CancellationTokenSource tokenSource)
        {
            _mockTaskService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), tokenSource.Token)).ReturnsAsync(task);

            var result = await _sut.GetByIdAsync(Guid.NewGuid(), tokenSource.Token);

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
            var routeResult = result.Result as OkObjectResult;
            Assert.AreEqual(200, routeResult.StatusCode);
            Assert.IsInstanceOf(typeof(TaskDetailViewModel), routeResult.Value);
        }

        [Test]
        [AutoData]
        public async Task GetByStatusAsync_WhenHaveTasks_ReturnsCorrectPagedResult(CancellationTokenSource tokenSource)
        {
            var completedTasks = new Fixture().Build<TaskDetailViewModel>().With(x => x.Completed, true).CreateMany(15);
            var pageTaskList = new PagedResult<TaskDetailViewModel>(completedTasks, 15, 1, 10);
            _mockTaskService.Setup(x => x.GetByStatusAsync(It.IsAny<bool>(), It.IsAny<PagingModel>(), tokenSource.Token)).ReturnsAsync(pageTaskList);

            var result = await _sut.GetByStatusAsync(new TaskStatusQuery { IsComplete = true, PageNumber = 1, PageSize = 10 }, tokenSource.Token);

            Assert.IsNotNull(result.Result);
            var routeResult = result.Result as OkObjectResult;
            Assert.AreEqual(200, routeResult.StatusCode);
            Assert.IsInstanceOf(typeof(PagedResult<TaskDetailViewModel>), routeResult.Value);
            var listResult = routeResult.Value as PagedResult<TaskDetailViewModel>;
            Assert.AreEqual(pageTaskList.TotalRecords, listResult.TotalRecords);
            Assert.AreEqual(pageTaskList.Data.Count(), listResult.Data.Count());
            Assert.AreEqual(pageTaskList.TotalPages, listResult.TotalPages);
        }

        [Test]
        [AutoData]
        public async Task GetByOwnersAsync_WhenHaveTasks_ReturnsCorrectPagedResult(Guid ownerId, CancellationTokenSource tokenSource)
        {
            var tasksByAnOwner = new Fixture().Build<TaskDetailViewModel>().With(x => x.OwnerId, ownerId).CreateMany(15);
            var pageTaskList = new PagedResult<TaskDetailViewModel>(tasksByAnOwner, 15, 1, 10);
            _mockTaskService.Setup(x => x.GetByOwnerAsync(It.IsAny<Guid>(), It.IsAny<PagingModel>(), tokenSource.Token)).ReturnsAsync(pageTaskList);

            var result = await _sut.GetByOwnerAsync(ownerId, new PagingModel { PageNumber = 1, PageSize = 10 }, tokenSource.Token);

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
            var routeResult = result.Result as OkObjectResult;
            Assert.AreEqual(200, routeResult.StatusCode);
            var listResult = routeResult.Value as PagedResult<TaskDetailViewModel>;
            Assert.AreEqual(pageTaskList.TotalRecords, listResult.TotalRecords);
            Assert.AreEqual(pageTaskList.Data.Count(), listResult.Data.Count());
            Assert.AreEqual(pageTaskList.TotalPages, listResult.TotalPages);
            Assert.AreEqual(10, listResult.Data.Count(x => x.OwnerId == ownerId));
        }

        [Test]
        [AutoData]
        public async Task MarkTaskCompleteAsync_WhenTaskIdPassed_ReturnsNoContentResult(Guid taskId, CancellationTokenSource tokenSource)
        {
            var result = await _sut.MarkTaskCompleteAsync(taskId, tokenSource.Token);

            Assert.IsInstanceOf(typeof(NoContentResult), result);
        }

        [Test]
        [AutoData]
        public async Task MarkTaskCompleteAsync_WhenTaskIdNotPassed_ReturnsBadResult(CancellationTokenSource tokenSource)
        {
            var id = Guid.Empty;

            var result = await _sut.MarkTaskCompleteAsync(id, tokenSource.Token);

            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
    }
}