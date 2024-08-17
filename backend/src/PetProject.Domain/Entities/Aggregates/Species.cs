using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.Domain.Entities.Aggregates
{
    public class Species : Shared.Entity<SpeciesId>
    {
        public string Name { get; private set; } = default!;
        public List<Breed> Breeds { get; private set; } = default!;

        private Species(SpeciesId id) : base(id)
        {

        }
    }
}
