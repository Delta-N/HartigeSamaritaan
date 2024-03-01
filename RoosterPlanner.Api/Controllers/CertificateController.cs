using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models.EntityViewModels;
using RoosterPlanner.Api.Models.HelperViewModels;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using RoosterPlanner.Service.Services;
using Type = RoosterPlanner.Api.Models.HelperViewModels.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ILogger<AvailabilityController> logger;
        private readonly ICertificateService certificateService;

        public CertificateController(ILogger<AvailabilityController> logger, ICertificateService certificateService)
        {
            this.logger = logger;
            this.certificateService = certificateService;
        }

        /// <summary>
        /// Makes a request towards the services layer for all CertificateTypes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("types")]
        public async Task<ActionResult<List<CertificateTypeViewModel>>> GetAllCertificateTypesAsync()
        {
            try
            {
                TaskListResult<CertificateType> result = await certificateService.GetAllCertificateTypesAsync();
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                if (result.Data.Count == 0)
                    return Ok(new List<CategoryViewModel>());

                List<CertificateTypeViewModel> certificateTypeViewModel = result.Data
                    .Select(CertificateTypeViewModel.CreateVm)
                    .ToList();

                return Ok(certificateTypeViewModel);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetAllCertificateTypesAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer for a specific CertificateType based on an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("types/{id}")]
        public async Task<ActionResult<CertificateTypeViewModel>> GetCertificateTypeAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<CertificateType> result = await certificateService.GetCertificateTypeAsync(id);

                if (!result.Succeeded)
                    if (result.Data == null)
                        return NotFound();
                CertificateTypeViewModel certificateTypeViewModel = CertificateTypeViewModel.CreateVm(result.Data);
                return Ok(certificateTypeViewModel);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetCertificateTypeAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to save a CertificateType.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPost("types")]
        public async Task<ActionResult<CertificateTypeViewModel>> SaveCertificateTypeAsync(CertificateTypeViewModel vm)
        {
            if (vm == null)
                return BadRequest("No valid CertificateType received");
            if (string.IsNullOrEmpty(vm.Name))
                return BadRequest("Name of a CertificateType cannot be empty");
            try
            {
                CertificateType certificateType = CertificateTypeViewModel.CreateCertificateType(vm);
                if (certificateType == null)
                    return BadRequest("Unable to convert CertificateTypeViewModel to CertificateType");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                certificateType.LastEditBy = oid;
                TaskResult<CertificateType> result;
                if (vm.Id == Guid.Empty)
                    result = await certificateService.CreateCertificateTypeAsync(certificateType);
                else
                    return BadRequest("Cannot update existing CertificateType with post method");

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(CertificateTypeViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveCertificateTypeAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to update a certificateType.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPut("types")]
        public async Task<ActionResult<CertificateTypeViewModel>> UpdateCertificateType(
            CertificateTypeViewModel vm)
        {
            if (vm == null)
                return BadRequest("No valid CertificateType received");
            if (string.IsNullOrEmpty(vm.Name))
                return BadRequest("Name of a CertificateType cannot be empty");
            try
            {
                CertificateType oldCertificateType = (await certificateService.GetCertificateTypeAsync(vm.Id)).Data;
                if (oldCertificateType == null)
                    return NotFound("CertificateType not found");
                if (!oldCertificateType.RowVersion.SequenceEqual(vm.RowVersion))
                    return BadRequest("Outdated entity received");

                CertificateType updatedCertificateType = CertificateTypeViewModel.CreateCertificateType(vm);
                if (updatedCertificateType == null)
                    return BadRequest("Unable to convert CertificateTypeViewModel to CertificateType");

                oldCertificateType.Name = updatedCertificateType.Name;
                oldCertificateType.Level = updatedCertificateType.Level;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldCertificateType.LastEditBy = oid;

                TaskResult<CertificateType>
                    result = await certificateService.UpdateCertificateTypeAsync(oldCertificateType);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(CertificateTypeViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateCertificateType);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// /// Makes a request towards the services layer to delete a CertificateType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpDelete("types/{id}")]
        public async Task<ActionResult<CertificateTypeViewModel>> RemoveCertificateTypeAsync(
            Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");

            try
            {
                CertificateType oldCertificateType = (await certificateService.GetCertificateTypeAsync(id)).Data;
                if (oldCertificateType == null)
                    return NotFound("CertificateType not found");

                TaskResult<CertificateType>
                    result = await certificateService.RemoveCertificateTypeAsync(oldCertificateType);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(CertificateTypeViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RemoveCertificateTypeAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// /// Makes a request towards the services layer get a specific certificate based on an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("certificate/{id}")]
        public async Task<ActionResult<CertificateViewModel>> GetCertificateAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Certificate> result = await certificateService.GetCertificateAsync(id);

                if (!result.Succeeded)
                    if (result.Data == null)
                        return NotFound();
                CertificateViewModel certificateTypeViewModel = CertificateViewModel.CreateVm(result.Data);
                return Ok(certificateTypeViewModel);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetCertificateAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to save a certificate.
        /// Only Boardmembers are allowed to save certificates.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPost("certificate")]
        public async Task<ActionResult<CertificateViewModel>> SaveCertificateAsync(CertificateViewModel vm)
        {
            if (vm == null)
                return BadRequest("No valid Certificate received");
            if (vm.Person == null || vm.CertificateType == null)
                return BadRequest("No valid Certificate received");
            try
            {
                Certificate certificate = CertificateViewModel.CreateCertificate(vm);
                if (certificate == null)
                    return BadRequest("Unable to convert CertificateViewModel to Certificate");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                certificate.LastEditBy = oid;
                certificate.CertificateType = null;
                certificate.Person = null;
                TaskResult<Certificate> result;
                if (vm.Id == Guid.Empty)
                    result = await certificateService.CreateCertificateAsync(certificate);
                else
                    return BadRequest("Cannot update existing Certificate with post method");

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(CertificateViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveCertificateAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to update a certificate.
        /// Only Boardmembers are allowd to update certificats. 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPut("certificate")]
        public async Task<ActionResult<CertificateViewModel>> UpdateCertificateAsync(CertificateViewModel vm)
        {
            if (vm == null)
                return BadRequest("No valid Certificate received");
            if (vm.Person == null || vm.CertificateType == null || vm.Id == Guid.Empty)
                return BadRequest("No valid Certificate received");

            try
            {
                Certificate oldCertificate = (await certificateService.GetCertificateAsync(vm.Id)).Data;
                if (oldCertificate == null)
                    return NotFound("Certificate not found");
                if (!oldCertificate.RowVersion.SequenceEqual(vm.RowVersion))
                    return BadRequest("Outdated entity received");
                if (oldCertificate.PersonId != vm.Person.Id)
                    return BadRequest("Cannot transfer ownership of a certificate");

                Certificate updatedCertificate = CertificateViewModel.CreateCertificate(vm);
                if (updatedCertificate == null)
                    return BadRequest("Unable to convert CertificateTypeViewModel to CertificateType");

                oldCertificate.DateIssued = updatedCertificate.DateIssued;
                oldCertificate.DateExpired = updatedCertificate.DateExpired;
                oldCertificate.CertificateTypeId = updatedCertificate.CertificateTypeId;
                oldCertificate.CertificateType = null;
                oldCertificate.Person = null;

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldCertificate.LastEditBy = oid;

                TaskResult<Certificate>
                    result = await certificateService.UpdateCertificateAsync(oldCertificate);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(CertificateViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateCertificateAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer delete a certificate.
        /// Only Boardmembers are allowed to delete  certificates.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpDelete("certificate/{id}")]
        public async Task<ActionResult<CertificateViewModel>> RemoveCertificateAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");

            try
            {
                Certificate oldCertificate = (await certificateService.GetCertificateAsync(id)).Data;
                if (oldCertificate == null)
                    return NotFound("Certificate not found");

                TaskResult<Certificate>
                    result = await certificateService.RemoveCertificateAsync(oldCertificate);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(CertificateViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RemoveCertificateTypeAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}