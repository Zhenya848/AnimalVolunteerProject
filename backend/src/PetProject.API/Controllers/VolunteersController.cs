using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService;
using PetProject.Domain.Shared;
using System.Linq;

namespace PetProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolunteersController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] IVolunteerService volunteerService,
            [FromBody] CreateVolunteerRequest createVolunteerRequest, 
            CancellationToken cancellationToken = default)
        {
            var result = await volunteerService.Create(createVolunteerRequest, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Created("", Envelope.Ok(result.Value).ToString());
        }   
    }
}