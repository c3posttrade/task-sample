using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskSample.Domain.Entities;
using TaskSample.Infrastructure.Persistence.EF.EntityConfiguration;

namespace TaskSample.Infrastructure.Persistence.EF
{
    public class SampleTaskDbContext : DbContext
    {
        public SampleTaskDbContext(DbContextOptions<SampleTaskDbContext> options) : base(options)
        {

        }
        public virtual DbSet<DemoTask> Tasks { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
            builder.ApplyConfigurationsFromAssembly(typeof(DemoTaskConfiguration).Assembly);
        }
    }
}
