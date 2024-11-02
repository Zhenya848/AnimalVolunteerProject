using PetProject.Core;
using PetProject.Core.ValueObjects.IdValueObjects;

namespace PetProject.Species.Domain
{
    public class Species : Entity<SpeciesId>
    {
        public string Name { get; private set; } = default!;
        public List<Breed> Breeds { get; private set; } = default!;

        private Species(SpeciesId id) : base(id)
        {

        }

        public Species(SpeciesId speciesId, string name, List<Breed> breeds) : base(speciesId)
        {
            Name = name;
            Breeds = breeds;
        }
    }
}
