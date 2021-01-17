using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Models;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Type = RoosterPlanner.Api.Models.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequirementsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> logger;
        private readonly IRequirementService requirementService;

        public RequirementsController(ILogger<ProjectsController> logger, IRequirementService requirementService)
        {
            this.logger = logger;
            this.requirementService = requirementService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequirementViewModel>> GetRequirementAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Requirement> result = await requirementService.GetRequirementAsync(id);

                if (!result.Succeeded)
                    if (result.Data == null)
                        return NotFound();
                RequirementViewModel requirementViewModel = RequirementViewModel.CreateVm(result.Data);
                return Ok(requirementViewModel);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetRequirementAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPost]
        public async Task<ActionResult<RequirementViewModel>> SaveRequirementAsync(
            RequirementViewModel requirementViewModel)
        {
            if (requirementViewModel == null)
                return BadRequest("No valid requirement received");
            if (requirementViewModel.Task == null || requirementViewModel.CertificateType == null)
                return BadRequest("No valid requirement received");

            try
            {
                Requirement requirement = (await requirementService.GetRequirementAsync(requirementViewModel.Task.Id,
                    requirementViewModel.CertificateType.Id)).Data;
                
                if(requirement!=null)
                    return BadRequest("Requirement already exists.");

                requirement = RequirementViewModel.CreateRequirement(requirementViewModel);
                if (requirement == null)
                    return BadRequest("Unable to convert RequirementViewModel to Requirement");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                requirement.LastEditBy = oid;
                requirement.CertificateType = null;
                requirement.Task = null;
                TaskResult<Requirement> result;
                if (requirementViewModel.Id == Guid.Empty)
                    result = await requirementService.CreateRequirement(requirement);
                else
                    return BadRequest("Cannot update existing Requirement with post method");

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(RequirementViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveRequirementAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut]
        public async Task<ActionResult<RequirementViewModel>> UpdateRequirementAsync(
            RequirementViewModel requirementViewModel)
        {
            if (requirementViewModel == null)
                return BadRequest("No valid Requirement received");
            if (requirementViewModel.Task == null || requirementViewModel.CertificateType == null ||
                requirementViewModel.Id == Guid.Empty)
                return BadRequest("No valid Certificate received");

            try
            {
                Requirement requirement = (await requirementService.GetRequirementAsync(requirementViewModel.Task.Id,
                    requirementViewModel.CertificateType.Id)).Data;

                if (requirement != null)
                    return BadRequest("Requirement already exists.");
                
                Requirement oldRequirement =
                    (await requirementService.GetRequirementAsync(requirementViewModel.Id)).Data;
                if (oldRequirement == null)
                    return NotFound("Requirement not found");
                if (!oldRequirement.RowVersion.SequenceEqual(requirementViewModel.RowVersion))
                    return BadRequest("Outdated entity received");
                if (oldRequirement.TaskId != requirementViewModel.Task.Id)
                    return BadRequest("Cannot update the task of a requirement. Use the delete and create methods");

                Requirement updatedRequirement = RequirementViewModel.CreateRequirement(requirementViewModel);
                if (updatedRequirement == null)
                    return BadRequest("Unable to convert RequirementViewModel to Requirement");

                oldRequirement.CertificateTypeId = updatedRequirement.CertificateTypeId;
                oldRequirement.CertificateType = null;
                oldRequirement.Task = null;

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldRequirement.LastEditBy = oid;

                TaskResult<Requirement>
                    result = await requirementService.UpdateRequirementAsync(oldRequirement);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(RequirementViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateRequirementAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RequirementViewModel>> DeleteRequirementAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");

            try
            {
                Requirement oldRequirement = (await requirementService.GetRequirementAsync(id)).Data;
                if (oldRequirement == null)
                    return NotFound("Requirement not found");

                TaskResult<Requirement>
                    result = await requirementService.RemoveCertificateAsync(oldRequirement);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(RequirementViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(DeleteRequirementAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}