using PetProject.Application.Shared.Interfaces.Commands;

namespace PetProject.Application.Volunteers.UseCases.Delete
{
    public record DeleteVolunteerCommand(
        Guid VolunteerId) : IDeleteCommand
    {

    };
}