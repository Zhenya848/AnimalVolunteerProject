using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Volunteers.ValueObjects.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetProject.Domain.Volunteers
{
    public class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
    {
        private bool _isDeleted = false;

        public FullName Name { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public TelephoneNumber TelephoneNumber { get; private set; } = default!;

        public Experience Experience { get; private set; } = default!;

        public RequisitesList RequisitesList { get; private set; } = default!;
        public SocialNetworksList SocialNetworksList { get; private set; } = default!;

        private List<Pet> _pets = new List<Pet>();
        public IReadOnlyList<Pet> Pets => _pets;

        private Volunteer(VolunteerId id) : base(id)
        {
        }

        public Volunteer(
            VolunteerId id, 
            FullName name, 
            Description description, 
            TelephoneNumber telephoneNumber, 
            Experience experience,
            List<SocialNetwork> sotialNetworks, 
            List<Requisite> requisites) : base(id)
        {
            Name = name;
            Description = description;
            TelephoneNumber = telephoneNumber;
            Experience = experience;

            RequisitesList = new RequisitesList(requisites);
            SocialNetworksList = new SocialNetworksList(sotialNetworks);
        }

        public void UpdateInfo(
            FullName name,
            Description description,
            TelephoneNumber telephoneNumber,
            Experience experience,
            List<SocialNetwork> sotialNetworks,
            List<Requisite> requisites)
        {
            Name = name;
            Description = description;
            TelephoneNumber = telephoneNumber;
            Experience = experience;
            RequisitesList.Requisites = requisites;
            SocialNetworksList.SocialNetworks = sotialNetworks;
        }

        public Result<Pet, Error> GetPetById(PetId id)
        {
            var pet = _pets.FirstOrDefault(p => p.Id == id);

            if (pet == null)
                return Errors.General.NotFound(id.Value);

            return pet;
        }

        public void Delete()
        {
            _isDeleted = true;
            DeleteSoftDeletableEntities(Pets);
        }
        public void Restore()
        {
            _isDeleted = false;
            RestoreSoftDeletableEntities(Pets);
        }

        private void DeleteSoftDeletableEntities(IReadOnlyList<ISoftDeletable> entities)
        {
            foreach (ISoftDeletable entity in entities)
                entity.Delete();
        }

        private void RestoreSoftDeletableEntities(IReadOnlyList<ISoftDeletable> entities)
        {
            foreach (ISoftDeletable entity in entities)
                entity.Restore();
        }

        public int CountOfShelterAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.FindAHome);
        public int CountOfHomelessAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.LookingForAHome);
        public int CountOfIllAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.NeedHelp);

        public UnitResult<Error> AddPet(Pet pet)
        {
            if (_pets.Count > 50)
                return Error.Failure("too.many.items", "You can't add more than 50 pets!");

            var serialNumber = SerialNumber.Create(_pets.Count + 1);

            if (serialNumber.IsFailure)
                return serialNumber.Error;

            pet.SetSerialNumber(serialNumber.Value);

            _pets.Add(pet);

            return Result.Success<Error>();
        }

        public UnitResult<Error> MovePet(Pet pet, SerialNumber serialNumber)
        {
            if (pet.SerialNumber == null || _pets.Select(p => p.Id == pet.Id).Any() == false)
                return Errors.General.NotFound(pet.Id);

            if (serialNumber.Value > _pets.Count)
                return Errors.General.ValueIsInvalid("Serial number");

            int indexOfPassedPet = pet.SerialNumber.Value - 1;
            int indexOfNewPet = serialNumber.Value - 1;

            if (indexOfPassedPet == indexOfNewPet)
                return Result.Success<Error>();

            pet.SetSerialNumber(serialNumber);
            Pet savedObj;

            if (indexOfPassedPet > indexOfNewPet)
            {
                _pets[indexOfNewPet].SerialNumber!.MoveSerialNumberToForward();

                savedObj = _pets[indexOfNewPet];
                _pets[indexOfNewPet] = pet;

                for (int i = indexOfNewPet + 1; i < indexOfPassedPet; i++)
                {
                    _pets[i].SerialNumber!.MoveSerialNumberToForward();
                    Pet current = _pets[i];

                    _pets[i] = savedObj;
                    savedObj = current;
                }
            }
            else
            {
                _pets[indexOfNewPet].SerialNumber!.MoveSerialNumberToBackward();

                savedObj = _pets[indexOfNewPet];
                _pets[indexOfNewPet] = pet;

                for (int i = indexOfNewPet - 1; i > indexOfPassedPet; i--)
                {
                    _pets[i].SerialNumber!.MoveSerialNumberToBackward();
                    Pet current = _pets[i];

                    _pets[i] = savedObj;
                    savedObj = current;
                }
            }

            _pets[indexOfPassedPet] = savedObj;

            return Result.Success<Error>();
        }
    }
}
