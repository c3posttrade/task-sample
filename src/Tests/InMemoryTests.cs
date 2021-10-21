using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Data.EF;
using TaskManager.Data.Model;

namespace Tests
{
    public class InMemoryTests
    {
        /// <summary>
        /// Just to confirm DB context/setup is right
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task BasicRun()
        {
            var factory = new DataContextFactory();
            using var context = factory.Build();
            var zero = new Job
            {
                Description = "sample local job",
                Status = JobStatus.Cancelled,
                Created = DateTime.UtcNow,
                Owner = Guid.NewGuid()
            };
            context.Jobs.Add(zero);
            var count = await context.SaveChangesAsync();
            Assert.AreEqual(1, count);

            var mine = context.Jobs.FirstOrDefault(x => x.Owner == zero.Owner);
            Assert.IsNotNull(mine);
            Assert.AreEqual(JobStatus.Cancelled, mine.Status);

            context.Jobs.Remove(mine);
            await context.SaveChangesAsync();
            var deleted = context.Jobs.FirstOrDefault(x => x.Owner == zero.Owner);
            Assert.IsNull(deleted);
        }
    }
}
