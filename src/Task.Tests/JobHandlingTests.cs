//using Core.Shared.EF;
//using NUnit.Framework;
//using Task.Core.Model;
//using Task.Data;
//using Task.Data.EF;
//using System.Threading.Tasks;

namespace Tests
{
    public class RandomTests
    {
        //private readonly Task.Data.IDataContextFactory _factory = new Task.Data.EF.DataContextFactory();

        //[Test]
        public async Task BasicRun()
        {
            //using var context = _factory.Build();
            var zero = new Task.Core.Model.Job
            {

            };
            //context.Jobs.AsTracked().Add(zero);
            //var id = await context.SaveChangesAsync();
        }
    }
}