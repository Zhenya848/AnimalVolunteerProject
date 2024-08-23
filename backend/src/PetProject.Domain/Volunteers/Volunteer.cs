using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Volunteers.ValueObjects.Collections;
using System.Collections.Generic;

namespace PetProject.Domain.Volunteers
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        public FullName Name { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public TelephoneNumber TelephoneNumber { get; private set; } = default!;

        public Experience EXP { get; private set; } = default!;

        public RequisitesList RequisitesList { get; private set; } = default!;
        public SocialNetworksList SocialNetworksList { get; private set; } = default!;

        public IReadOnlyList<Pet> Pets = default!;

        private Volunteer(VolunteerId id) : base(id)
        {
        }

        public Volunteer(VolunteerId id, 
            FullName name, 
            Description description, 
            TelephoneNumber telephoneNumber, 
            Experience exp,
            List<SocialNetwork> sotialNetworks, 
            List<Requisite> requisites) : base(id)
        {
            Name = name;
            Description = description;
            TelephoneNumber = telephoneNumber;
            EXP = exp;

            RequisitesList = new RequisitesList(requisites);
            SocialNetworksList = new SocialNetworksList(sotialNetworks);
        }

        public int CountOfShelterAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.FindAHome);
        public int CountOfHomelessAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.LookingForAHome);
        public int CountOfIllAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.NeedHelp);
    }
}
