using PetProject.Domain.Shared.ValueObjects.IdClasses;

namespace PetProject.Domain.Species
{
    public class Breed : Shared.Entity<BreedId>
    {
        public string title { get; private set; } = default!;

        protected Breed(BreedId id) : base(id) { }
    }
}
