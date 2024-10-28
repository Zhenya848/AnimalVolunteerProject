using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Application.Volunteers.UseCases.Create;
using PetProject.Application.Volunteers.UseCases.Update;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Application.Volunteers.UseCases.Delete;
using PetProject.API.Controllers.Volunteers.Requests;
using PetProject.Application.Volunteers.Pets.Commands.Create;
using PetProject.Application.Volunteers.Pets.Commands.UploadPhotos;
using PetProject.Application.Files.Commands.Get;
using PetProject.Application.Files.Commands.Delete;
using PetProject.Application.Files.Commands.Create;
using PetProject.Application.Volunteers.Commands.Create;
using PetProject.Application.Volunteers.Commands.Update;
using PetProject.Application.Volunteers.Commands.Delete;
using PetProject.Application.Volunteers.Queries;
using PetProject.Application.Volunteers.Commands.Get;
using PetProject.Application.Volunteers.Pets.Commands.Update;
using PetProject.API.Controllers.Pets;

namespace PetProject.API.Controllers.Volunteers
{
    public class VolunteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateVolunteerCommand(request.Name, request.Description, request.PhoneNumber,
                request.Experience, request.SotialNetworks, request.Requisites);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut("{id:guid}/volunteer-info")]
        public async Task<ActionResult<Guid>> UpdateVolunteerInfo(
            [FromServices] UpdateVolunteerHandler handler,
            [FromBody] UpdateVolunteerRequest request,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateVolunteerCommand(id, request.Name, request.Description,
                request.PhoneNumber, request.Experience, request.SocialNetworks, request.Requisites);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> Delete(
            [FromServices] DeleteVolunteerHandler handler,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteVolunteerCommand(id);

            var result = await handler.Handle(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult> GetWithPagination(
            [FromServices] GetVolunteersWithPaginationHandler handler,
            [FromQuery] GetVolunteersWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetVolunteersWithPaginationQuery(
                request.Page,
                request.PageSize,
                request.OrderByDesc,
                request.OrderBy);

            var responseResult = await handler.Handle(query, cancellationToken);
            
            if (responseResult.IsFailure)
                return responseResult.Error.ToResponse();
            
            return Ok(Envelope.Ok(responseResult.Value));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Get(
            [FromServices] GetVolunteerHandler handler,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(id, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("petPhoto")]
        public async Task<IActionResult> CreatePhoto(
            IFormFile file,
            [FromServices] CreateFilesHandler handler,
            CancellationToken cancellationToken)
        {
            await using var stream = file.OpenReadStream();
            var request = new CreateFilesCommand([new FileData(stream, Guid.NewGuid().ToString())], "photos");

            var result = await handler.Create(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }

        [HttpDelete("petPhoto/{objectName:guid}")]
        public async Task<IActionResult> DeletePhoto(
            [FromRoute] Guid objectName,
            [FromServices] DeleteFileHandler handler,
            CancellationToken cancellationToken)
        {
            DeleteFileCommand request = new DeleteFileCommand("photos", objectName.ToString());
            var result = await handler.Delete(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet("petPhoto/{objectName:guid}")]
        public async Task<IActionResult> GetPhoto(
            [FromRoute] Guid objectName,
            [FromServices] GetFileHandler handler,
            CancellationToken cancellationToken)
        {
            GetFileCommand request = new GetFileCommand("photos", objectName.ToString());
            var result = await handler.Get(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{volunteerId:guid}/pet")]
        public async Task<IActionResult> CreatePet(
            [FromServices] CreatePetHandler handler,
            [FromBody] CreatePetRequest request,
            [FromRoute] Guid volunteerId,
            CancellationToken cancellationToken)
        {
            var command = PetCommandsInitializer
                .InitializeCreatePetCommand(volunteerId, request);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut("{volunteerId:guid}/{petId:guid}/pet")]
        public async Task<IActionResult> UpdatePet(
            [FromServices] UpdatePetHandler handler,
            [FromBody] UpdatePetRequest request,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            CancellationToken cancellationToken)
        {
            var command = PetCommandsInitializer
                .InitializeUpdatePetCommand(volunteerId, petId, request);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
        public async Task<IActionResult> UploadPhotosToPet(
            [FromServices] UploadFilesToPetHandler handler,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            CancellationToken cancellationToken)
        {
            await using FormFileProcessor formFileProcessor = new FormFileProcessor();
            List<UploadFileDto> filesDto = formFileProcessor.StartProcess(files);

            var command = new UploadFilesToPetCommand(volunteerId, petId, filesDto);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }
    }
}