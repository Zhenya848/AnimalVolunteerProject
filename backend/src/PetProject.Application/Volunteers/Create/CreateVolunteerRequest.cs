using PetProject.Domain.Shared.ValueObjects.Dtos;

namespace PetProject.Application.Volunteers.Create
{
    public record CreateVolunteerRequest(
        FullNameDto Name, 
        string Description, 
        string PhoneNumber, 
        int Experience, 
        IEnumerable<SocialNetworkDto> SotialNetworks, 
        IEnumerable<RequisiteDto> Requisites) 
    {

    };
}