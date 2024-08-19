using Microsoft.AspNetCore.Mvc;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Services;
using PetProject.Domain.Entities.Aggregates;
using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolunteersController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromServices] IVolunteerService volunteerService, 
            [FromBody] CreateVolunteerRequest createVolunteerRequest)
        {
            var result = await volunteerService.Create(createVolunteerRequest);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
