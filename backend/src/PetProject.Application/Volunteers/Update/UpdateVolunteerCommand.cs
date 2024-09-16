using PetProject.Domain.Shared.ValueObjects.Dtos;

namespace PetProject.Application.Volunteers.Update
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