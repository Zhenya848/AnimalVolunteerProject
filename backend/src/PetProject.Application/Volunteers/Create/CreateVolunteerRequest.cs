using PetProject.Domain.Shared.ValueObjects.Dtos;

namespace PetProject.Application.Volunteers.Create
{
    public record CreateVolunteerRequest(
        FullNameDto name, 
        string description, 
        string phoneNumber, 
        int experience, 
        IEnumerable<SocialNetworkDto> sotialNetworks, 
        IEnumerable<RequisiteDto> requisites) 
    {

    };
}