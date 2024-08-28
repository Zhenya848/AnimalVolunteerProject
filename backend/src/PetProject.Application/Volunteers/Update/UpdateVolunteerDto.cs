using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Application.Volunteers.Update
{
    public record UpdateVolunteerDto(
        FullNameDto Name,
        string Description,
        string PhoneNumber,
        int Experience,
        IEnumerable<SocialNetworkDto> SotialNetworks,
        IEnumerable<RequisiteDto> Requisites)
    {

    };
}