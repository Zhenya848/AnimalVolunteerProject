using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Core.ValueObjects.IdValueObjects;

namespace PetProject.Species.Domain
{
    public class Species : Core.Entity<SpeciesId>
    {
        public string Name { get; private set; } = default!;
        public List<Breed> Breeds { get; private set; } = default!;

        private Species(SpeciesId id) : base(id)
        {

        }

        private Species(SpeciesId speciesId, string name, List<Breed> breeds) : base(speciesId)
        {
            Name = name;
            Breeds = breeds;
        }

        public static Result<Species, Error> Create(string speciesName, List<Breed>? breeds = null)
        {
            if (string.IsNullOrWhiteSpace(speciesName))
                return Error.Validation("value.name.empty", "Species name is required");

            return new Species(SpeciesId.AddNewId(), speciesName, breeds ?? []);
        }
    }
}
