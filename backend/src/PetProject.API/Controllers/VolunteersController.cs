using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService;
using PetProject.Application.Volunteers.Update;
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

            return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut("{id:guid}/volunteer-info")]
        public async Task<ActionResult<Guid>> Put(
            [FromServices] IVolunteerService volunteerService,
            [FromServices] IValidator<UpdateVolunteerRequest> validator,
            [FromBody] UpdateVolunteerDto dto,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var request = new UpdateVolunteerRequest(id, dto);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var result = await volunteerService.Update(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> Delete(
            [FromServices] IVolunteerService volunteerService,
            [FromServices] IValidator<DeleteVolunteerRequest> validator,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteVolunteerRequest(id);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var result = await volunteerService.Delete(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}