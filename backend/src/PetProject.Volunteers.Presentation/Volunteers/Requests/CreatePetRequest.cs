using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Volunteers.Presentation.Volunteers.Requests
{
    public record CreatePetRequest(
        Guid SpeciesId,
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
