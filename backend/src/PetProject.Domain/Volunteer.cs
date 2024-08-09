using System;
using System.Collections.Generic;
using System.Linq;
namespace PetProject.Domain
{
    public class Volunteer
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string TelephoneNumber { get; private set; }

        public int EXP { get; private set; }
        public int CountOfShelterAnimals { get; private set; }
        public int CountOfHomelessAnimals { get; private set; }
        public int CountOfIllAnimals { get; private set; }

        public List<SotialNetwork> SotialNetworks { get; private set; }
        public List<Requisite> Requisites { get; private set; }
        public List<Pet> Pets { get; private set; }
    }
}
