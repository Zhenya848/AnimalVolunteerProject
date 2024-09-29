using PetProject.Application.Volunteers.UseCases.Pets.Create;
using PetProject.Domain.Shared.ValueObjects.Dtos;

namespace PetProject.API.Controllers.Volunteers.Requests
{
    public record CreatePetRequest(
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
