using PetProject.Domain.Entities;

namespace PetProject.Domain.Aggregates
{
    public class Volunteer
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string TelephoneNumber { get; private set; } = default!;

        public int EXP { get; private set; }
        public int CountOfShelterAnimals { get; private set; }
        public int CountOfHomelessAnimals { get; private set; }
        public int CountOfIllAnimals { get; private set; }

        public List<SotialNetwork> SotialNetworks { get; private set; } = default!;
        public List<Requisite> Requisites { get; private set; } = default!;
        public List<Pet> Pets { get; private set; } = default!;
    }
}
