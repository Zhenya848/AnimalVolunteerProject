namespace PetProject.Application.Volunteers.Update
{
    public record UpdateVolunteerRequest(
        Guid volunteerId, UpdateVolunteerDto dto)
    {

    };
}