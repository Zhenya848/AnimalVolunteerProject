using PetProject.API.Controllers.Volunteers.Requests;
using PetProject.Application.Volunteers.Pets.Commands.Create;
using PetProject.Application.Volunteers.Pets.Commands.Update;

namespace PetProject.API.Controllers.Pets
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
