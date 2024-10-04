using PetProject.Application.Shared.Interfaces.Commands;

namespace PetProject.Application.Volunteers.Pets.Commands.Update
{
    public record DeletePetCommand(Guid PetId) : IDeleteCommand;
}
