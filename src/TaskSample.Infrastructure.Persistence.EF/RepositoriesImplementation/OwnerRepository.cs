using TaskSample.Domain.Entities;
using TaskSample.Domain.Repositories;

namespace TaskSample.Infrastructure.Persistence.EF.RepositoriesImplementation
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(SampleTaskDbContext context) : base(context)
        {
        }
    }
}
