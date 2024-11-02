using PetProject.Core.ValueObjects.IdValueObjects;

namespace PetProject.Volunteers.Domain.ValueObjects
{
    public record PetTypeInfo
    {
        public BreedId BreedId { get; } = null!;
        public SpeciesId SpeciesId { get; } = null!;

        public PetTypeInfo(BreedId breedId, SpeciesId speciesId)
        {
            BreedId = breedId;
            SpeciesId = speciesId;
        }
    }
}
