using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Domain;
using TaskSample.Domain.Entities;
using TaskSample.Shared;

namespace TaskSample.Infrastructure.Persistence.EF.Tests
{
    public class TaskRepositoryTests
    {
        UnitOfWork _sut;
        Mock<IDateTimeProvider> _dateTimeProvider;
        SampleTaskDbContext _context = null;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SampleTaskDbContext>().UseInMemoryDatabase("MemoryTaskDatabase").Options;
            _dateTimeProvider = new();
            _context = new SampleTaskDbContext(options);
            _context.Database.EnsureCreated();
            _sut = new UnitOfWork(_context, _dateTimeProvider.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddAsync_WhenTaskAdded_StoreCorrectInformation()
        {
            //Arrange
            CancellationTokenSource tokenSource = new();
            var currentDateTime = DateTimeOffset.Now;
            _dateTimeProvider.Setup(x => x.DateTimeNow).Returns(currentDateTime);
            var newTask = new DemoTask { Description = "Test task description", OwnerId = Guid.NewGuid() };

            //Act
            var result = await _sut.TaskRepository.AddAsync(newTask, tokenSource.Token);
            await _sut.SaveChangesAsync(tokenSource.Token);

            //Assert
            Assert.Greater(await _context.Tasks.CountAsync(tokenSource.Token), 0);
            var taskCreated = await _context.Tasks.FirstAsync(tokenSource.Token);
            Assert.IsNotNull(taskCreated.Id);
            Assert.AreEqual(currentDateTime, taskCreated.Created);
            Assert.IsNull(taskCreated.Updated);
        }

        [Test]
        [AutoData]
        public async Task GetByIdAsync_ReturnsCorrectInformation(DemoTask task)
        {
            CancellationTokenSource tokenSource = new();
            await _context.Tasks.AddAsync(task, tokenSource.Token);
            await _context.SaveChangesAsync(tokenSource.Token);

            var result = await _sut.TaskRepository.FindByIdAsync(task.Id, tokenSource.Token);

            Assert.IsNotNull(result);
            Assert.AreEqual(task.Id, result.Id);
            Assert.AreEqual(task.OwnerId, result.OwnerId);
            Assert.AreEqual(task.Description, result.Description);
            Assert.AreEqual(task.Created, result.Created);
        }

        [Test]
        public async Task GetByOwnerIdAsync_ReturnsCorrectResult()
        {
            CancellationTokenSource tokenSource = new();
            Owner owner = new() { Id = Guid.NewGuid(), Name = "Test Owner" };
            var tasks = new Fixture().Build<DemoTask>().With(x => x.OwnerId, owner.Id).With(x => x.Owner, owner).CreateMany(13);
            await _context.Tasks.AddRangeAsync(tasks, tokenSource.Token);
            await _context.SaveChangesAsync(tokenSource.Token);
            DataPaging dataPaging = new() { Skip = 1, Take = 10 };

            var result = await _sut.TaskRepository.GetByOwnerIdAsync(owner.Id, dataPaging, tokenSource.Token);

            Assert.IsNotNull(result);
            Assert.AreEqual(13, result.TotalRecords);
            Assert.AreEqual(10, result.FilteredRecords.Count());
        }


        [Test]
        public async Task GetByStatusAsync_ReturnsCorrectResult()
        {
            CancellationTokenSource tokenSource = new();
            var tasks = new Fixture().Build<DemoTask>().With(x => x.IsCompleted, true).CreateMany(25);
            await _context.Tasks.AddRangeAsync(tasks, tokenSource.Token);
            await _context.SaveChangesAsync(tokenSource.Token);
            DataPaging dataPaging = new() { Skip = 1, Take = 10 };

            var result = await _sut.TaskRepository.GetByStatusAsync(true, dataPaging, tokenSource.Token);

            Assert.IsNotNull(result);
            Assert.AreEqual(25, result.TotalRecords);
            Assert.AreEqual(10, result.FilteredRecords.Count(x => x.IsCompleted));
        }


    }
}
