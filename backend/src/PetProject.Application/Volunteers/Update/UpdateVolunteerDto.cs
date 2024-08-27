using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Application.Volunteers.Update
{
    public record UpdateVolunteerDto(
        FullNameDto name,
        string description,
        string phoneNumber,
        int experience,
        IEnumerable<SocialNetworkDto> sotialNetworks,
        IEnumerable<RequisiteDto> requisites)
    {

    };
}