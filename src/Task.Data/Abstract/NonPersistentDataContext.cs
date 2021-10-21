using Shared.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Data.Model;

namespace TaskManager.Data.Abstract
{
    public class NonPersistentDataContext : IDataContext
    {
        private bool disposedValue;

        public NonPersistentDataContext()
        {
            Jobs = new NonPersistentDataTable<Job>();
            AuditLogs = new NonPersistentDataTable<AuditLog>();
        }

        public NonPersistentDataContext(List<Job> jobs)
        {
            Jobs = new NonPersistentDataTable<Job>(jobs);
            AuditLogs = new NonPersistentDataTable<AuditLog>();
        }

        public Task<bool> CanConnectAsync(CancellationToken ct = default)
        {
            return Task.FromResult(true);
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return Task.FromResult(1);
        }

        public IDataTable<Job> Jobs { get; set; }
        public IDataTable<AuditLog> AuditLogs { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
