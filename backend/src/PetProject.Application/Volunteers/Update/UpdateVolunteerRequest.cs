namespace PetProject.Application.Volunteers.Update
{
    public record UpdateVolunteerRequest(
        Guid VolunteerId, UpdateVolunteerDto Dto)
    {

    };
}