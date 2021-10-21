using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Data.EF.Extensions
{
    public static class DataContextExtensions
    {
        public static void AddDataServices(this IServiceCollection services, string connection)
        {
            services.AddSingleton<IDataContextFactory, DataContextFactory>((_) => new DataContextFactory { ConnectionString = connection });
        }
    }
}
