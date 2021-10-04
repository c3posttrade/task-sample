using AutoMapper;
using TaskSample.Domain;
using TaskSample.Services.Common.Paging;

namespace TaskSample.Infrastructure.Services.MappingProfiles
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<PagingModel, DataPaging>()
                .ForMember(dest => dest.Skip, opt => opt.MapFrom(src => (src.PageNumber - 1) * src.PageSize))
                .ForMember(dest => dest.Take, opt => opt.MapFrom(src => src.PageSize));
        }
    }
}
