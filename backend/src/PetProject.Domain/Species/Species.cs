﻿using PetProject.Domain.Shared.ValueObjects.IdClasses;

namespace PetProject.Domain.Species
{
    public class Species : Shared.Entity<SpeciesId>
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
