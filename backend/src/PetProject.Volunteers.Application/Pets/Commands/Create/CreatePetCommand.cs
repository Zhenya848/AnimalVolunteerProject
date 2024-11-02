using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Volunteers.Application.Pets.Commands.Create
{
    public record CreatePetCommand(
        Guid VolunteerId,
        Guid SpeciseId,
        Guid BreedId,
        string Name,
        string Description,
        string Color,
        string HealthInfo,
        AddresDto Addres,
        string TelephoneNumber,
        float Weight,
        float Height,
        bool IsCastrated,
        bool IsVaccinated,
        DateTime BirthdayTime,
        DateTime DateOfCreation,
        IEnumerable<RequisiteDto> Requisites,
        HelpStatus HelpStatus);
}
