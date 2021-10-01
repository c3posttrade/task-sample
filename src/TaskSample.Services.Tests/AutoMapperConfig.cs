using AutoMapper;
using TaskSample.Infrastructure.Services.MappingProfiles;

namespace TaskSample.Infrastructure.Services.Tests
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration GetConfiguration()
        {
            return new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TaskProfile());
                mc.AddProfile(new DataProfile());
            });
        }
    }
}
