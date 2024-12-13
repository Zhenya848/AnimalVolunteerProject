namespace PetProject.Species.Application.Breeds.Commands;

public record CreateBreedCommand(Guid SpeciesId, string BreedName);