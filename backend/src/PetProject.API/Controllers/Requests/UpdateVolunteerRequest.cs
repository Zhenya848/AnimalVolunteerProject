using PetProject.Domain.Shared.ValueObjects.Dtos;

namespace PetProject.API.Controllers.Requests
{
    public record UpdateVolunteerRequest(
        FullNameDto Name,
        string Description,
        string PhoneNumber,
        int Experience,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<RequisiteDto> Requisites);
}
