using PetProject.Volunteers.Application.Pets.Commands.Create;
using PetProject.Volunteers.Application.Pets.Commands.Update;
using PetProject.Volunteers.Presentation.Volunteers.Requests;

namespace PetProject.Volunteers.Presentation.Pets
{
    public static class PetCommandsInitializer
    {
        public static CreatePetCommand InitializeCreatePetCommand(
            Guid volunteerId,
            CreatePetRequest request) =>
            new CreatePetCommand(volunteerId, request.SpeciesId, request.BreedId, request.Name, request.Description,
                request.Color, request.HealthInfo, request.Addres, request.TelephoneNumber,
                request.Weight, request.Height, request.IsCastrated, request.IsVaccinated,
                request.BirthdayTime, request.DateOfCreation, request.Requisites, request.HelpStatus);

        public static UpdatePetCommand InitializeUpdatePetCommand(
            Guid volunteerId,
            Guid petId,
            UpdatePetRequest request) =>
            new UpdatePetCommand(petId, volunteerId, request.SpeciesId, request.BreedId, request.Name, request.Description,
                request.Color, request.HealthInfo, request.Addres, request.TelephoneNumber,
                request.Weight, request.Height, request.IsCastrated, request.IsVaccinated,
                request.BirthdayTime, request.DateOfCreation, request.Requisites, request.HelpStatus);
    }
}
