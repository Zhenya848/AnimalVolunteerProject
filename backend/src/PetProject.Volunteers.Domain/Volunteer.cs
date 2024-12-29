using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Volunteers.Domain.ValueObjects;

namespace PetProject.Volunteers.Domain
{
    public class Volunteer : SoftDeletableEntity<VolunteerId>
    {
        public FullName Name { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public TelephoneNumber TelephoneNumber { get; private set; } = default!;

        public Experience Experience { get; private set; } = default!;

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
            Experience experience) : base(id)
        {
            Name = name;
            Description = description;
            TelephoneNumber = telephoneNumber;
            Experience = experience;
        }

        public void UpdateInfo(
            FullName name,
            Description description,
            TelephoneNumber telephoneNumber,
            Experience experience)
        {
            Name = name;
            Description = description;
            TelephoneNumber = telephoneNumber;
            Experience = experience;
        }

        public Result<Pet, Error> GetPetById(PetId id)
        {
            var pet = _pets.FirstOrDefault(p => p.Id == id);

            if (pet == null)
                return Errors.General.NotFound(id.Value);

            return pet;
        }

        public override void Delete()
        {
            base.Delete();
            
            foreach (var entity in Pets)
                entity.Delete();
        }
        public override void Restore()
        {
            base.Restore();
            
            foreach (var entity in Pets)
                entity.Restore();
        }

        public int CountOfShelterAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.FindAHome);
        public int CountOfHomelessAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.LookingForAHome);
        public int CountOfIllAnimals() => Pets.Count(p => p.HelpStatus == HelpStatus.NeedHelp);

        public UnitResult<Error> AddPet(Pet pet)
        {
            var serialNumber = SerialNumber.Create(_pets.Count + 1);

            if (serialNumber.IsFailure)
                return serialNumber.Error;

            pet.SetSerialNumber(serialNumber.Value);

            _pets.Add(pet);

            return Result.Success<Error>();
        }

        public UnitResult<Error> UpdatePetInfo(
            Guid petId,
            string name,
            Description description,
            string color,
            string healthInfo,
            Address address,
            TelephoneNumber telephoneNumber,
            float weight,
            float height,
            bool isCastrated,
            bool isVaccinated,
            DateTime birthdayTime,
            DateTime dateOfCreation,
            List<Requisite> requisites,
            SpeciesId speciesId,
            BreedId breedId,
            HelpStatus helpStatus)
        {
            var pet = _pets.FirstOrDefault(i => i.Id == petId);

            if (pet == null)
                return Errors.General.NotFound(petId);

            pet.UpdateInfo(
                name,
                description,
                color,
                healthInfo,
                address,
                telephoneNumber,
                weight,
                height,
                isCastrated,
                isVaccinated,
                birthdayTime,
                dateOfCreation,
                requisites,
                speciesId,
                breedId,
                helpStatus);

            return Result.Success<Error>();
        }

        public UnitResult<Error> MovePet(Pet pet, SerialNumber serialNumber)
        {
            if (_pets.Select(p => p.Id == pet.Id).Any() == false)
                return Errors.General.NotFound(pet.Id);

            if (serialNumber > _pets.Count)
                return Errors.General.ValueIsInvalid("Serial number");

            int indexOfPassedPet = pet.SerialNumber - 1;
            int indexOfNewPet = serialNumber - 1;

            if (indexOfPassedPet == indexOfNewPet)
                return Result.Success<Error>();

            pet.SetSerialNumber(serialNumber);
            Pet savedObj;

            if (indexOfPassedPet > indexOfNewPet)
            {
                _pets[indexOfNewPet].MoveSerialNumberToForward();

                savedObj = _pets[indexOfNewPet];
                _pets[indexOfNewPet] = pet;

                for (int i = indexOfNewPet + 1; i < indexOfPassedPet; i++)
                {
                    _pets[i].MoveSerialNumberToForward();
                    Pet current = _pets[i];

                    _pets[i] = savedObj;
                    savedObj = current;
                }
            }
            else
            {
                _pets[indexOfNewPet].MoveSerialNumberToBackward();

                savedObj = _pets[indexOfNewPet];
                _pets[indexOfNewPet] = pet;

                for (int i = indexOfNewPet - 1; i > indexOfPassedPet; i--)
                {
                    _pets[i].MoveSerialNumberToBackward();
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
