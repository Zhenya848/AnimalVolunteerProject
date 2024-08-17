using PetProject.Domain.Entities;

namespace PetProject.Domain.Aggregates
{
    public class Species : Shared.Entity<SpeciesId>
    {
        public string PetType { get; private set; } = default!;
        public List<Breed> Breeds { get; private set; } = default!;

        private Species(SpeciesId id) : base(id)
        {

        }
    }
}
