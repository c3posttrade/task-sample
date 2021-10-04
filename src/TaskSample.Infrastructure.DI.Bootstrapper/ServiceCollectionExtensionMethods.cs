using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using TaskSample.Infrastructure.Persistence.EF.ExtensionMethods;
using TaskSample.Infrastructure.Services.ExtensionMethods;

namespace TaskSample.Infrastructure.DI.Bootstrapper
{
    public static class ServiceCollectionExtensionMethods
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddSharedServices();
            services.AddServices();
            services.AddPersistence();
        }
        public static async Task SeedTaskData(this IServiceProvider serviceProvider)
        {
            await serviceProvider.SeedData();
        }
    }
}
