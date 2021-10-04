using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TaskSample.Domain;
using TaskSample.Domain.Entities;
using TaskSample.Domain.Repositories;
using TaskSample.Infrastructure.Persistence.EF.RepositoriesImplementation;

namespace TaskSample.Infrastructure.Persistence.EF.ExtensionMethods
{
    public static class ServiceCollectionnMethods
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<SampleTaskDbContext>(options => options.UseInMemoryDatabase("MemoryTaskDatabase"));

            services
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddTransient<ITaskRepository, TaskRepository>()
                .AddTransient<IOwnerRepository, OwnerRepository>();
        }
        public static async Task SeedData(this IServiceProvider serviceProvider)
        {
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            Guid ownerId = await SeedOwnerData(unitOfWork);
            await SeedTaskData(unitOfWork, ownerId);
        }

        private static async Task SeedTaskData(IUnitOfWork unitOfWork, Guid ownerId)
        {
            await unitOfWork.TaskRepository.AddAsync(new DemoTask
            {
                Id = new Guid("1916efea-1ce8-4275-9a4d-1aa71bceff30"),
                IsCompleted = false,
                OwnerId = ownerId,
                Description = "Sample Task 1"
            });
            await unitOfWork.TaskRepository.AddAsync(new DemoTask
            {
                Id = new Guid("a5a3ca1d-a990-4188-849d-c11cacf00618"),
                IsCompleted = true,
                OwnerId = ownerId,
                Description = "Sample Task 2",
                Updated = DateTimeOffset.Now
            });
            await unitOfWork.SaveChangesAsync();
        }

        private static async Task<Guid> SeedOwnerData(IUnitOfWork unitOfWork)
        {
            var ownerId = new Guid("064fbceb-1872-4234-ba85-f4075264ebd3");
            await unitOfWork.OwnerRepository.AddAsync(new Owner { Id = ownerId, Name = "Task Owner 1" });
            await unitOfWork.SaveChangesAsync();
            return ownerId;
        }
    }
}
