using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data.EF.Abstract
{
    public class InMemoryDataContext : DataContext
    {
        public InMemoryDataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public InMemoryDataContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // add here any custom setup for in memory db
            base.OnModelCreating(builder);
        }
    }
}
