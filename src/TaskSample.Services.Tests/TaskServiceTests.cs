using AutoFixture;
using AutoFixture.NUnit3;
using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Domain;
using TaskSample.Domain.Entities;
using TaskSample.Infrastructure.Services.Features;
using TaskSample.Services.Common.Paging;
using TaskSample.Services.Exceptions;
using TaskSample.Services.Features.Tasks.Models;
using TaskSample.Shared;

namespace TaskSample.Infrastructure.Services.Tests
{
    public class TaskServiceTests
    {
        private readonly TaskService _sut;
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public TaskServiceTests()
        {
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _sut = new TaskService(_mockUnitOfWork.Object, _mockDateTimeProvider.Object, GetAllMappingProfiles());
        }
        private static IMapper GetAllMappingProfiles()
        {
            IMapper mapper = AutoMapperConfig.GetConfiguration().CreateMapper();
            return mapper;
        }

        [Test]
        [AutoData]
        public void CreateAsync_WhenOwnerNotFound_ReturnsNotFoundException(TaskCreateModel taskcreateModel, CancellationTokenSource tokenSource)
        {
            _mockUnitOfWork.Setup(x => x.OwnerRepository.FindByIdAsync(It.IsAny<Guid>(), tokenSource.Token)).ReturnsAsync((Owner)null);

            Assert.That(async () =>
            {
                await _sut.CreateAsync(taskcreateModel, tokenSource.Token);
            }, Throws.TypeOf<NotFoundException>().With.Message.EqualTo($"Owner with Id: {taskcreateModel.OwnerId} doesn't exist"));
        }

        [Test]
        [AutoData]
        public async Task CreateAsync_WhenOwnerFound_ReturnsCreatedTask(DemoTask newTask, CancellationTokenSource tokenSource)
        {
            //Arrange
            TaskCreateModel taskcreateModel = new() { Description = newTask.Description, OwnerId = newTask.OwnerId };
            _mockDateTimeProvider.Setup(x => x.DateTimeNow).Returns(newTask.Created);
            _mockUnitOfWork.Setup(x => x.OwnerRepository.FindByIdAsync(It.IsAny<Guid>(), tokenSource.Token)).ReturnsAsync(new Owner());
            _mockUnitOfWork.Setup(x => x.TaskRepository.AddAsync(It.IsAny<DemoTask>(), tokenSource.Token)).ReturnsAsync(newTask);

            //Act
            var result = await _sut.CreateAsync(taskcreateModel, tokenSource.Token);

            //Assert
            Assert.AreEqual(newTask.Id, result.Id);
            Assert.AreEqual(newTask.Created, result.Created);
            Assert.AreEqual(newTask.Owner.Name, result.OwnerName);
        }

        [Test]
        [AutoData]
        public void GetByIdAsync_WhenTaskNotFound_ReturnsNotFoundException(Guid id, CancellationTokenSource tokenSource)
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.TaskRepository.FindByIdAsync(It.IsAny<Guid>(), tokenSource.Token)).ReturnsAsync((DemoTask)null);

            //Assert
            Assert.That(async () =>
            {
                //Act
                await _sut.GetByIdAsync(id, tokenSource.Token);
            }, Throws.TypeOf<NotFoundException>().With.Message.EqualTo($"Task with Id: {id} not found"));

        }

        [Test]
        [AutoData]
        public async Task GetByIdAsync_WhenTaskFound_ReturnsCorrectValues(DemoTask expectedTask, CancellationTokenSource tokenSource)
        {
            _mockUnitOfWork.Setup(x => x.TaskRepository.FindByIdAsync(It.IsAny<Guid>(), tokenSource.Token)).ReturnsAsync(expectedTask);

            var resultTask = await _sut.GetByIdAsync(expectedTask.Id, tokenSource.Token);

            Assert.IsNotNull(resultTask);
            Assert.AreEqual(expectedTask.Id, resultTask.Id);
        }

        [Test]
        [AutoData]
        public async Task GetByOwnerAsync_ReturnsCorrectResult(Guid ownerId, CancellationTokenSource tokenSource)
        {
            //Arrange
            var tasks = new Fixture().Build<DemoTask>().With(x => x.OwnerId, ownerId).CreateMany(10);
            DataResult<DemoTask> result = new DataResult<DemoTask> { FilteredRecords = tasks, TotalRecords = 15 };
            _mockUnitOfWork.Setup(x => x.TaskRepository.GetByOwnerIdAsync(It.IsAny<Guid>(), It.IsAny<DataPaging>(), tokenSource.Token)).ReturnsAsync(result);

            //Act
            var taskList = await _sut.GetByOwnerAsync(ownerId, new PagingModel { PageNumber = 1, PageSize = 10 }, tokenSource.Token);

            //Assert
            Assert.IsNotNull(taskList);
            Assert.AreEqual(15, taskList.TotalRecords);
            Assert.AreEqual(2, taskList.TotalPages);
            Assert.AreEqual(10, taskList.PageSize);
        }

        [Test]
        [AutoData]
        public async Task GetByStatusAsync_ReturnsCorrectResult(bool isComplete, CancellationTokenSource tokenSource)
        {
            //Arrange
            var tasks = new Fixture().Build<DemoTask>().With(x => x.IsCompleted, isComplete).CreateMany(10);
            DataResult<DemoTask> result = new DataResult<DemoTask> { FilteredRecords = tasks, TotalRecords = 40 };
            _mockUnitOfWork.Setup(x => x.TaskRepository.GetByStatusAsync(isComplete, It.IsAny<DataPaging>(), tokenSource.Token)).ReturnsAsync(result);

            //Act
            var taskList = await _sut.GetByStatusAsync(isComplete, new PagingModel { PageNumber = 1, PageSize = 15 }, tokenSource.Token);

            //Assert
            Assert.IsNotNull(taskList);
            Assert.AreEqual(40, taskList.TotalRecords);
            Assert.AreEqual(3, taskList.TotalPages);
            Assert.AreEqual(15, taskList.PageSize);
        }

        [Test]
        [AutoData]
        public void MarkCompleteAsync_WhenTaskNotFound_ReturnsNotFoundException(Guid id, CancellationTokenSource tokenSource)
        {
            _mockUnitOfWork.Setup(x => x.TaskRepository.FindByIdAsync(It.IsAny<Guid>(), tokenSource.Token)).ReturnsAsync((DemoTask)null);

            Assert.That(async () =>
            {
                await _sut.MarkCompleteAsync(id, tokenSource.Token);
            }, Throws.TypeOf<NotFoundException>().With.Message.EqualTo($"Task with Id: {id} not found"));

        }
    }
}
