using Shared.Core.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Data.Model;

namespace TaskManager.Data
{
    public interface IDataContext : IDisposable
    {
        Task<bool> CanConnectAsync(CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        IDataTable<Job> Jobs { get; }
        IDataTable<AuditLog> AuditLogs { get; }
    }
}
