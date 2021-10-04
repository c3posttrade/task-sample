using Microsoft.Extensions.DependencyInjection;
using TaskSample.Infrastructure.Services.Features;
using TaskSample.Infrastructure.Services.MappingProfiles;
using TaskSample.Infrastructure.Services.Shared;
using TaskSample.Services.Features.Tasks;
using TaskSample.Shared;

namespace TaskSample.Infrastructure.Services.ExtensionMethods
{
    public static class ServiceCollectionMethods
    {
        //for the demo sake, I haven't created base service for the services, which will help to automatically scan and configure
        //the services, no need to add manaually everytime you create new one
        public static void AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TaskProfile));
            services.AddTransient<ITaskService, TaskService>();
        }
        public static void AddSharedServices(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        }
    }
}
