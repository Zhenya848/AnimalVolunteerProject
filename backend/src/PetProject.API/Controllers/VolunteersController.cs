using Microsoft.AspNetCore.Mvc;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService;

namespace PetProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolunteersController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromServices] ICRUDVolunteerService volunteerService, 
            [FromBody] CreateVolunteerRequest createVolunteerRequest, CancellationToken cancellationToken = default)
        {
            var result = await volunteerService.Create(createVolunteerRequest, cancellationToken);

            if (result.IsFailure)
                return result.ToResponse();

            return result.ToResponse();
        }
    }
}