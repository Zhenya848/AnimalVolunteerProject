using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Volunteers.Presentation.Volunteers.Requests
{
    public record CreateVolunteerRequest(
        FullNameDto Name,
        string Description,
        string PhoneNumber,
        int Experience);
}
