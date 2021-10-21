using Microsoft.EntityFrameworkCore;
using Shared.Core.Persistence;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Data.Model;

namespace TaskManager.Data.EF
{
    public class DataContext : DbContext, IDataContext
    {
        private static DbContextOptions GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql(connectionString)
                .Options;
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        public Task<bool> CanConnectAsync(CancellationToken ct = default)
        {
            return Database.CanConnectAsync(ct);
        }

        #region DbContext
        IDataTable<Job> IDataContext.Jobs { get { return new PersistentDataTable<Job>(Jobs); } }
        IDataTable<AuditLog> IDataContext.AuditLogs { get { return new PersistentDataTable<AuditLog>(AuditLogs); } }
        #endregion

        #region EF
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        #endregion
    }
}
