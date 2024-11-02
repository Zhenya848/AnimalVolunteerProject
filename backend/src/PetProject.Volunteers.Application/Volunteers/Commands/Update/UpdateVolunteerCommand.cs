using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Volunteers.Application.Volunteers.Commands.Update
{
    public record UpdateVolunteerCommand(
        Guid VolunteerId,
        FullNameDto Name,
        string Description,
        string PhoneNumber,
        int Experience,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<RequisiteDto> Requisites)
    {

    };
}