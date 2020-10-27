﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Api.Models.Enums;
using RoosterPlanner.Common;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service;
using Extensions = RoosterPlanner.Api.Models.Constants.Extensions;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IPersonService personService;
        private readonly IProjectService projectService;

        //Constructor
        public PersonsController(IPersonService personService, IProjectService projectService,
            ILogger logger)
        {
            this.personService = personService;
            this.projectService = projectService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest("No valid id.");

                var result = await personService.GetUser(id);

                if (!result.Succeeded) return UnprocessableEntity();

                var personVm = PersonViewModel.CreateVm(result.Data);
                return Ok(personVm);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController - Get(Guid id): Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        //Mogen project admins (medewerkercommissie) ook alle gebruikers opvragen?)
        [Authorize(Policy = "Boardmember")]
        [HttpGet]
        public async Task<ActionResult> Get(string email, string firstName, string lastName, string userRole,
            string city, int offset = 0, int pageSize = 20)
        {
            //TODO aanpassen zodra dat nodig is
            var filter = new PersonFilter(offset, pageSize)
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                UserRole = userRole,
                City = city
            };

            var personViewModels = new List<PersonViewModel>();

            try
            {
                var result = await personService.GetB2CMembers(filter);

                if (!result.Succeeded) return UnprocessableEntity();

                personViewModels.AddRange(result.Data.Select(user => PersonViewModel.CreateVm(user)));

                return Ok(personViewModels);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController - Get(): Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateUser([FromBody] PersonViewModel personViewModel)
        {
            try
            {
                if (personViewModel == null) return BadRequest("Invalid User");

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                string oid = null;
                if (identity != null)
                    oid = identity.Claims.FirstOrDefault(c =>
                            c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                        ?.Value;

                if (oid == null) return BadRequest("Invalid User");

                var currentUserActionResult = Get(Guid.Parse(oid));
                var okObjectResult = (OkObjectResult) currentUserActionResult.Result;

                //only the owner of a profile or a boardmember can update user data
                if (okObjectResult.Value is PersonViewModel currentUser &&
                    (personViewModel.Id.ToString() == oid ||
                     currentUser.UserRole == UserRole.Boardmember.ToString()))
                {
                    var user = PersonViewModel.CreateUser(personViewModel);
                    var result = await personService.UpdatePerson(user, personViewModel.Id);
                    if (result.Succeeded) return Ok(result);
                }
                else
                {
                    return Unauthorized();
                }

                return BadRequest("Invalid User");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController - UpdateUser(User user): Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPatch("modifyadmin/{oid}/{modifier}")]
        public async Task<ActionResult> ModAdmin(Guid oid, int modifier)
        {
            try
            {
                if (oid == Guid.Empty) return BadRequest("No valid id.");

                //check if user exists
                var result = await personService.GetUser(oid);

                if (!result.Succeeded) return UnprocessableEntity();

                var user = new User
                {
                    AdditionalData = new Dictionary<string, object>()
                };


                user.AdditionalData.Add(Extensions.UserRoleExtension, modifier);
                result = await personService.UpdatePerson(user, Guid.Parse(result.Data.Id));
                if (result.Succeeded)
                    return Ok(result.Data);

                return UnprocessableEntity();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController - Patch(): Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }
    }
}