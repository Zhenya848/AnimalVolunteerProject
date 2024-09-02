using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Delete;
using PetProject.Application.Files.Get;
using PetProject.Application.Files.Services;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService;
using PetProject.Application.Volunteers.Update;
using PetProject.Domain.Shared;
using System.Linq;

namespace PetProject.API.Controllers
{
    public class VolunteersController : ApplicationController
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
        public async Task<ActionResult<Guid>> UpdateVolunteerInfo(
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

        [HttpPost("petPhoto")]
        public async Task<IActionResult> CreatePhoto(
            IFormFile file,
            [FromServices] IFileService service,
            CancellationToken cancellationToken)
        {
            await using var stream = file.OpenReadStream();
            var request = new CreateFileRequest("photos", Guid.NewGuid().ToString(), stream);

            var result = await service.Create(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("petPhoto/{objectName:guid}")]
        public async Task<IActionResult> DeletePhoto(
            [FromRoute] Guid objectName,
            [FromServices] IFileService service,
            CancellationToken cancellationToken)
        {
            DeleteFileRequest request = new DeleteFileRequest("photos", objectName.ToString());
            var result = await service.Delete(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet("petPhoto/{objectName:guid}")]
        public async Task<IActionResult> GetPhoto(
            [FromRoute] Guid objectName,
            [FromServices] IFileService service,
            CancellationToken cancellationToken)
        {
            GetFileRequest request = new GetFileRequest("photos", objectName.ToString());
            var result = await service.Get(request, cancellationToken);

            if (result.IsFailure) 
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}