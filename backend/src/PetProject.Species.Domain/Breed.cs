using PetProject.Core;
using PetProject.Core.ValueObjects.IdValueObjects;

namespace PetProject.Species.Domain
{
    public class Breed : Entity<BreedId>
    {
        public string title { get; private set; } = default!;

        protected Breed(BreedId id) : base(id) { }
    }
}
