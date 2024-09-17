using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Requests;
using PetProject.API.Response;
using PetProject.API.Volunteers;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Delete;
using PetProject.Application.Files.Get;
using PetProject.Application.Files.Services;
using PetProject.Application.Pets.Create;
using PetProject.Application.Pets.Services;
using PetProject.Application.Pets.UploadPhotos;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Services;
using PetProject.Application.Volunteers.Update;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Volunteers.ValueObjects;
using System.Linq;

namespace PetProject.API.Controllers
{
    public class VolunteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] IVolunteerService volunteerService,
            [FromBody] CreateVolunteerRequest request, 
            CancellationToken cancellationToken = default)
        {
            var command = new CreateVolunteerCommand(request.Name, request.Description, request.PhoneNumber,
                request.Experience, request.SotialNetworks, request.Requisites);

            var result = await volunteerService.Create(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut("{id:guid}/volunteer-info")]
        public async Task<ActionResult<Guid>> UpdateVolunteerInfo(
            [FromServices] IVolunteerService volunteerService,
            [FromBody] UpdateVolunteerRequest request,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateVolunteerCommand(id, request.Name, request.Description, 
                request.PhoneNumber, request.Experience, request.SocialNetworks, request.Requisites);

            var result = await volunteerService.Update(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> Delete(
            [FromServices] IVolunteerService volunteerService,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteVolunteerCommand(id);

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
            var request = new CreateFilesCommand([ new FileData(stream, Guid.NewGuid().ToString()) ], "photos");

            var result = await service.Create(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }

        [HttpDelete("petPhoto/{objectName:guid}")]
        public async Task<IActionResult> DeletePhoto(
            [FromRoute] Guid objectName,
            [FromServices] IFileService service,
            CancellationToken cancellationToken)
        {
            DeleteFileCommand request = new DeleteFileCommand("photos", objectName.ToString());
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
            GetFileCommand request = new GetFileCommand("photos", objectName.ToString());
            var result = await service.Get(request, cancellationToken);

            if (result.IsFailure) 
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{volunteerId:guid}/pet")]
        public async Task<IActionResult> CreatePet(
            [FromServices] IPetService service,
            [FromBody] CreatePetRequest request,
            [FromRoute] Guid volunteerId,
            CancellationToken cancellationToken)
        {
            var command = InitializePetCommand(volunteerId, request);

            var result = await service.Create(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
        }

        private CreatePetCommand InitializePetCommand(Guid volunteerId, CreatePetRequest request) =>
            new CreatePetCommand(volunteerId, request.Name, request.Description,
                request.Color, request.HealthInfo, request.Addres, request.TelephoneNumber,
                request.Weight, request.Height, request.IsCastrated, request.IsVaccinated,
                request.BirthdayTime, request.DateOfCreation, request.Requisites, request.HelpStatus);


        [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
        public async Task<IActionResult> UploadPhotosToPet(
            [FromServices] IPetService service,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            CancellationToken cancellationToken)
        {
            await using FormFileProcessor formFileProcessor = new FormFileProcessor();
            List<UploadFileDto> filesDto = formFileProcessor.StartProcess(files);

            var command = new UploadFilesToPetCommand(volunteerId, petId, filesDto);

            var result = await service.UploadFiles(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }
    }
}