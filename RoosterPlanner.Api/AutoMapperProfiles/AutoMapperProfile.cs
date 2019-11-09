using AutoMapper;
using RoosterPlanner.Api.Models;

namespace RoosterPlanner.Api.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RoosterPlanner.Models.Project, ProjectViewModel>();

            CreateMap<RoosterPlanner.Models.Project, ProjectDetailsViewModel>();

            CreateMap<RoosterPlanner.Models.Task, TaskViewModel>();

            CreateMap<RoosterPlanner.Models.Shift, ShiftViewModel>()
                .ForMember(i => i.Category, opt => opt.MapFrom(src => src.Task.Category.Name))
                .ForMember(i => i.Name, opt => opt.MapFrom(src => src.Task.Name));
        }
    }
}
