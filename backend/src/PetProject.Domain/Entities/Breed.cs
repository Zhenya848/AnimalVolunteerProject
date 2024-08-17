using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.Domain.Entities
{
    public class Breed : Shared.Entity<BreedId>
    {
        public string title { get; private set; } = default!;

        protected Breed(BreedId id) : base(id) { }
    }
}
