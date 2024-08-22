using PetProject.Domain.Shared.ValueObjects.IdClasses;

namespace PetProject.Domain.Species
{
    public record PetTypeInfo
    {
        public BreedId BreedId { get; } = null!;
        public SpeciesId SpeciesId { get; } = null!;

        private PetTypeInfo(BreedId breedId, SpeciesId speciesId)
        {
            BreedId = breedId;
            SpeciesId = speciesId;
        }
    }
}
