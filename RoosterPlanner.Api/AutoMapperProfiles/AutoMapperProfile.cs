using AutoMapper;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Project, ProjectViewModel>();

            CreateMap<Project, ProjectDetailsViewModel>();

            CreateMap<Task, TaskViewModel>();

            CreateMap<Shift, ShiftViewModel>()
                .ForMember(i => i.Category, opt => opt.MapFrom(src => src.Task.Category.Name))
                .ForMember(i => i.Name, opt => opt.MapFrom(src => src.Task.Name));
        }
    }
}
