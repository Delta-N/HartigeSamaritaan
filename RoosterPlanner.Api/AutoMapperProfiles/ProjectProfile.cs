using AutoMapper;

namespace RoosterPlanner.Api.AutoMapperProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<RoosterPlanner.Models.Project, Models.Projects>();
                //.ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
