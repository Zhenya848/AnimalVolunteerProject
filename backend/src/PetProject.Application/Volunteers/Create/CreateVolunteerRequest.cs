using PetProject.Domain.Dtos;

namespace PetProject.Application.Volunteers.Create
{
    public record CreateVolunteerRequest(string firstname, string lastName, string patronymic, string description, 
        string phoneNumber, int exp, List<SocialNetworkDto> sotialNetworks, List<RequisiteDto> requisites) 
    {

    };
}