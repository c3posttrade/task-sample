using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskSample.Domain;
using TaskSample.Domain.Repositories;
using TaskSample.Infrastructure.Persistence.EF.RepositoriesImplementation;
using TaskSample.Shared;

namespace TaskSample.Infrastructure.Persistence.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SampleTaskDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UnitOfWork(SampleTaskDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dateTimeProvider = dateTimeProvider;
        }
        public ITaskRepository TaskRepository => new TaskRepository(_context);

        public IOwnerRepository OwnerRepository => new OwnerRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken token = default)
        {
            var addedModifiedEntities = _context.ChangeTracker.Entries().Where(x => (x.State is EntityState.Added or EntityState.Modified));
            foreach (var entry in addedModifiedEntities)
            {
                var entityType = entry.Context.Model.FindEntityType(entry.Entity.GetType());
                if (entry.State == EntityState.Modified)
                {
                    const string modifiedOnField = "Updated";
                    var modifiedOnProperty = entityType.FindProperty(modifiedOnField);
                    if (modifiedOnProperty != null)
                    {
                        entry.Property(modifiedOnField).CurrentValue = _dateTimeProvider.DateTimeNow;
                    }
                }

                if (entry.State == EntityState.Added)
                {
                    const string createdOnField = "Created";
                    var createdOnProperty = entityType.FindProperty(createdOnField);                 
                    if (createdOnProperty != null)
                    {
                        entry.Property(createdOnField).CurrentValue = _dateTimeProvider.DateTimeNow;
                    }
                }
            }

            return await _context.SaveChangesAsync(token).ConfigureAwait(false);
        }

    }
}
