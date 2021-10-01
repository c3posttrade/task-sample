using NUnit.Framework;

namespace TaskSample.Infrastructure.Services.Tests
{
    public class MapperTests
    {
        [Test]
        public void Validate_AutoMapper_Profiles()
        {
            AutoMapperConfig.GetConfiguration().AssertConfigurationIsValid();
        }
    }
}
