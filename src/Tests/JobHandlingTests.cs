using NSubstitute;
using NUnit.Framework;
using Shared.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManager.Core.DTO;
using TaskManager.Core.Services;
using TaskManager.Data;
using TaskManager.Data.Abstract;
using TaskManager.Data.Model;

namespace Tests
{
    public class JobHandlingTests
    {
        static IDataContextFactory SetupContext(List<Job> jobs)
        {
            var context = new NonPersistentDataContext(jobs);
            var factory = Substitute.For<IDataContextFactory>();
            factory.Build().Returns(context);
            return factory;
        }


        [Test]
        public async Task FindNone()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var saved = await service.GetByIdAsync(Guid.Empty);
            Assert.AreEqual(ResponseStatus.NotFound, saved.Status);
        }

        [Test]
        public async Task Create()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var model = new TaskModel
            {
                Description = "Random",
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Owner = Guid.NewGuid()
            };
            await service.CreateAsync(model);
            var saved = await service.GetByIdAsync(model.Id);
            Assert.AreEqual(model.Description, saved.Value.Description);
            Assert.AreEqual(model.Owner, saved.Value.Owner);
        }

        [Test]
        public async Task CreateOnce()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var model = new TaskModel
            {
                Description = "Random",
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Owner = Guid.NewGuid()
            };

            for (int i = 0; i < 3; i++)
            {
                var added = await service.CreateAsync(model);
                var expected = i == 0 ? ResponseStatus.OK : ResponseStatus.BadRequest;
                Assert.AreEqual(expected, added.Status);

                var saved = await service.GetByIdAsync(model.Id);
                Assert.AreEqual(model.Description, saved.Value.Description);
                Assert.AreEqual(model.Owner, saved.Value.Owner);
            }
        }

        [Test]
        public async Task CreateNullEmpty()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var saved = await service.CreateAsync(null);
            Assert.IsFalse(saved.Value);
            Assert.AreEqual(ResponseStatus.BadRequest, saved.Status);
        }

        [Test]
        public async Task CreateNoOwner()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var model = new TaskModel
            {
                Id = Guid.NewGuid(),
                Description = "empty owner",
                Owner = Guid.Empty
            };
            await service.CreateAsync(model);
            var saved = await service.CreateAsync(null);
            Assert.IsFalse(saved.Value);
            Assert.AreEqual(ResponseStatus.BadRequest, saved.Status);
        }


        [Test]
        public async Task Update()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var model = new TaskModel
            {
                Id = Guid.NewGuid(),
                Description = "Random",
                Created = DateTime.UtcNow,
                Owner = Guid.NewGuid()
            };
            await service.CreateAsync(model);
            Console.WriteLine(JsonSerializer.Serialize(model));

            model.Description = "Oops I did it again";
            model.Completed = true;

            await service.UpdateAsync(model);
            var saved = await service.GetByIdAsync(model.Id);
            Console.WriteLine(JsonSerializer.Serialize(saved));

            Assert.AreEqual(model.Description, saved.Value.Description);
            Assert.AreEqual(model.Owner, saved.Value.Owner);
            Assert.IsNotNull(saved.Value.Updated);
        }

        [Test]
        public async Task UpdateNullEmpty()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var model = new TaskModel
            {
                Id = Guid.NewGuid(),
                Description = "Random",
                Created = DateTime.UtcNow,
                Owner = Guid.NewGuid()
            };
            await service.CreateAsync(model);
            Console.WriteLine(JsonSerializer.Serialize(model));

            await service.UpdateAsync(null);
            var saved = await service.GetByIdAsync(model.Id);
            Console.WriteLine(JsonSerializer.Serialize(saved));

            Assert.IsNull(saved.Value.Updated);
        }

        [Test]
        public async Task GetPagedResults()
        {
            var random = new List<Job>();
            var me = Guid.NewGuid();

            for (int i = 0; i < 101; i++)
            {
                random.Add(new Job
                {
                     Owner = me, 
                     Description = $"Random task {i}",
                     TaskId = Guid.NewGuid(),
                     Created = DateTime.UtcNow
                });
            }

            var factory = SetupContext(jobs: random);
            var service = new TaskManagerService(factory);

            for (int i = 0; i < 4; i++)
            {
                var paging = new PageConfig
                {
                    Page = i,
                    PageSize = 50
                };
                var result = await service.GetByOwnerAsync(me, paging);
                var items = result.Value;
                Assert.AreEqual(paging.Page, items.CurrentPage);
                Assert.IsTrue(items.Items.Count <= paging.PageSize);
                Console.WriteLine($"Page {items.CurrentPage}/{items.TotalPages} contains {items.Items.Count}");
                if (items.Items.Count > 0)
                {
                    var any = items.Items.FirstOrDefault();
                    Console.WriteLine($"Sample {JsonSerializer.Serialize(any)}");
                }
            }
        }

        [Test]
        public async Task GetPagedResultsEmpty()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var paging = new PageConfig
            {
            };
            var items = await service.GetByOwnerAsync(Guid.Empty, paging);
            Assert.AreEqual(items.Value.CurrentPage, -1);
        }

        [Test]
        public async Task GetPagedResultsNullCheck()
        {
            var factory = SetupContext(jobs: new List<Job>());
            var service = new TaskManagerService(factory);
            var items = await service.GetByOwnerAsync(Guid.Empty, null);
            Assert.AreEqual(items.Value.CurrentPage, -1);
        }

    }
}
