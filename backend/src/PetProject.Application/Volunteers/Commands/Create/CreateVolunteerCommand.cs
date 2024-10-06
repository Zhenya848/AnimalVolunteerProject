using PetProject.Domain.Shared.ValueObjects.Dtos;

namespace PetProject.Application.Volunteers.UseCases.Create
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