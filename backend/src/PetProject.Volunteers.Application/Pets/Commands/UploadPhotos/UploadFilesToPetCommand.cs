using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Volunteers.Application.Pets.Commands.UploadPhotos
{
    public record UploadFilesToPetCommand(
        Guid VolunteerId,
        Guid PetId,
        IEnumerable<UploadFileDto> Files);
}
