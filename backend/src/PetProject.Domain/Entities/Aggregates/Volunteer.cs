using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.ValueObjects;
using PetProject.Domain.ValueObjects.Details;
using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.Domain.Entities.Aggregates
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        public FullName Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public TelephoneNumber TelephoneNumber { get; private set; } = default!;

        public int EXP { get; private set; }

        public VolunteerDetails Details { get; private set; } = default!;
        public List<Pet> Pets { get; private set; } = default!;

        private Volunteer(VolunteerId id) : base(id)
        {
        }

        private Volunteer(VolunteerId id, FullName name, string description, TelephoneNumber telephoneNumber, int exp, 
            List<SocialNetwork> sotialNetworks, List<Requisite> requisites, List<Pet> pets) : base(id)
        {
            Name = name;
            Description = description;
            TelephoneNumber = telephoneNumber;
            EXP = exp;

            Details = new VolunteerDetails(sotialNetworks, requisites);
            Pets = pets;
        }

        public static Result<Volunteer, Error> Create(VolunteerId id, FullName fullName, 
            string description, TelephoneNumber telephoneNumber, int exp, List<SocialNetwork> sotialNetworks, 
            List<Requisite> requisites, List<Pet> pets)
        {
            return new Volunteer(id, fullName, description, telephoneNumber, exp, 
                sotialNetworks, requisites, pets);
        }

        public int CountOfShelterAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.FindAHome);
        public int CountOfHomelessAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.LookingForAHome);
        public int CountOfIllAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.NeedHelp);
    }
}
