using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Core.ValueObjects.IdValueObjects;

namespace PetProject.Species.Domain
{
    public class Breed : Core.Entity<BreedId>
    {
        public string Title { get; private set; } = default!;

        protected Breed(BreedId id) : base(id) { }

        private Breed(BreedId id, string title) : base(id)
        {
            Title = title;
        }

        public static Result<Breed, Error> Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Error.Validation("value.name.empty", "Breed name is required");
            
            return new Breed(BreedId.AddNewId(), title);
        }
    }
}
