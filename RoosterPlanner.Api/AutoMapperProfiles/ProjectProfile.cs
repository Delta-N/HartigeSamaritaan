using AutoMapper;
using RoosterPlanner.Api.Models;

namespace RoosterPlanner.Api.AutoMapperProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<RoosterPlanner.Models.Project, ProjectViewModel>();

            CreateMap<RoosterPlanner.Models.Project, ProjectDetailsViewModel>();

            CreateMap<RoosterPlanner.Models.Task, TaskViewModel>();
        }
    }
}
