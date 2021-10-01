using AutoMapper;
using TaskSample.Domain.Entities;
using TaskSample.Services.Features.Tasks.Models;

namespace TaskSample.Infrastructure.Services.MappingProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskCreateModel, DemoTask>()
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Updated, opt => opt.Ignore());

            CreateMap<DemoTask, TaskDetailViewModel>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.IsCompleted));
        }
    }
}
