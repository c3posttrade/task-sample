using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskSample.Domain.Entities;

namespace TaskSample.Infrastructure.Persistence.EF.EntityConfiguration
{
    public class DemoTaskConfiguration : IEntityTypeConfiguration<DemoTask>
    {
        public void Configure(EntityTypeBuilder<DemoTask> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.IsCompleted).HasDefaultValue(false);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Created).IsRequired();

            builder.ToTable("Tasks", DatabaseSchema.Task);
        }
    }
}
