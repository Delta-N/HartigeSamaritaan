using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Service;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        protected IMapper Mapper { get; set; }
        public IProjectService ProjectService { get; set; }
        public IParticipationService ParticipationService { get; set; }
        public IAzureB2CService AzureB2CService { get; set; }
        public IPersonService PersonService { get; set; }
        public IShiftService ShiftService { get; set; }
        public IMatchService MatchService { get; set; }
    }
}