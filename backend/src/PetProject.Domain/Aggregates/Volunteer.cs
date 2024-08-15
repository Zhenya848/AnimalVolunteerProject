using PetProject.Domain.Entities;
using PetProject.Domain.ValueObjects;

namespace PetProject.Domain.Aggregates
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        public FullName Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string TelephoneNumber { get; private set; } = default!;

        public int EXP { get; private set; }
        public int CountOfShelterAnimals { get; private set; }
        public int CountOfHomelessAnimals { get; private set; }
        public int CountOfIllAnimals { get; private set; }

        public List<SotialNetwork> SotialNetworks { get; private set; } = default!;
        public List<Requisite> Requisites { get; private set; } = default!;
        public List<Pet> Pets { get; private set; } = default!;

        private Volunteer(VolunteerId id) : base(id)
        {
        }
    }
}
