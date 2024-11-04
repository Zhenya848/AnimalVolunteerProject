using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.Core.ValueObjects.Dtos;
using PetProject.Framework;
using PetProject.Framework.Authorization;
using PetProject.Volunteers.Application.Files.Commands.Create;
using PetProject.Volunteers.Application.Files.Commands.Delete;
using PetProject.Volunteers.Application.Files.Commands.Get;
using PetProject.Volunteers.Application.Pets.Commands.Create;
using PetProject.Volunteers.Application.Pets.Commands.Update;
using PetProject.Volunteers.Application.Pets.Commands.UploadPhotos;
using PetProject.Volunteers.Application.Volunteers.Commands.Create;
using PetProject.Volunteers.Application.Volunteers.Commands.Delete;
using PetProject.Volunteers.Application.Volunteers.Commands.Get;
using PetProject.Volunteers.Application.Volunteers.Commands.Update;
using PetProject.Volunteers.Application.Volunteers.Queries;
using PetProject.Volunteers.Presentation.Pets;
using PetProject.Volunteers.Presentation.Volunteers.Requests;

namespace PetProject.Volunteers.Presentation.Volunteers
{
    public class VolunteersController : ApplicationController
    {
        [HttpPost]
        [Permission("volunteer.create")]
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
        [Permission("volunteer.update")]
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
        [Permission("volunteer.delete")]
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
        [Permission("volunteer.get")]
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