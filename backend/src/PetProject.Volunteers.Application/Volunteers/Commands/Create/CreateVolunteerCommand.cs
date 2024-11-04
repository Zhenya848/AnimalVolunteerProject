using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Volunteers.Application.Volunteers.Commands.Create
{
    public record CreateVolunteerCommand(
        FullNameDto Name,
        string Description,
        string PhoneNumber,
        int Experience,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<RequisiteDto> Requisites)
    {

    };
}