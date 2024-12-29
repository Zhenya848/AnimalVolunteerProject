using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Volunteers.Presentation.Volunteers.Requests
{
    public record UpdateVolunteerRequest(
        FullNameDto Name,
        string Description,
        string PhoneNumber,
        int Experience);
}
