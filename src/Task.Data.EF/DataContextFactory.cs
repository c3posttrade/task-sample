using Microsoft.EntityFrameworkCore;
using TaskManager.Data.EF.Abstract;

namespace TaskManager.Data.EF
{
    public class DataContextFactory : IDataContextFactory
    {
        public string ConnectionString { get; set; }

        public IDataContext Build()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(databaseName: "RAM")
                    .Options;
                return new InMemoryDataContext(options);
            }
            return new DataContext(ConnectionString);
        }
    }
}
