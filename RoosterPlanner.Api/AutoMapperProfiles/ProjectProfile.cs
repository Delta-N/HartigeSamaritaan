using AutoMapper;
using RoosterPlanner.Api.Models;

namespace RoosterPlanner.Api.AutoMapperProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<RoosterPlanner.Models.Project, ProjectViewModel>();
            //.ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<RoosterPlanner.Models.Task, TaskViewModel>();
        }
    }
}
