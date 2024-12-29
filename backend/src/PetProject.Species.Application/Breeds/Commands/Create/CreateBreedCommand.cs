namespace PetProject.Species.Application.Breeds.Commands.Create;

public record CreateBreedCommand(Guid SpeciesId, string BreedName);