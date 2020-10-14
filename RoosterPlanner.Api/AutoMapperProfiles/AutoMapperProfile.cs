using AutoMapper;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Api.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RoosterPlanner.Models.Project, ProjectViewModel>();

            CreateMap<RoosterPlanner.Models.Project, ProjectDetailsViewModel>();

            CreateMap<Task, TaskViewModel>();

            CreateMap<Shift, ShiftViewModel>()
                .ForMember(i => i.Category, opt => opt.MapFrom(src => src.Task.Category.Name))
                .ForMember(i => i.Name, opt => opt.MapFrom(src => src.Task.Name));
        }
    }
}
