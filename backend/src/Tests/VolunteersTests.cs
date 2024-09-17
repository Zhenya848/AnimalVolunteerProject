using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;

namespace Tests
{
    public class VolunteersTests
    {
        [Fact]
        public void Add_First_Pet_Init_Return_Success_Result()
        {
            const string TEST = "Test";

            var telephoneNumber = TelephoneNumber.Create("+79482251131").Value;

            var fullName = FullName.Create(
            TEST,
            TEST,
            TEST).Value;

            var description = Description.Create(TEST).Value;
            var experience = Experience.Create(0).Value;

            var socilaNetworkDto = new SocialNetworkDto(TEST, TEST);
            var requisiteDto = new RequisiteDto(TEST, TEST);

            var socialNetworks = new List<SocialNetwork>()
            { SocialNetwork.Create(socilaNetworkDto.name, socilaNetworkDto.reference).Value };

            var requisites = new List<Requisite>()
            { Requisite.Create(requisiteDto.Title, requisiteDto.Description).Value };

            var addres = Addres.Create(
                TEST,
                TEST,
                TEST,
                TEST).Value;

            var speciesId = SpeciesId.AddEmptyId();
            var breedId = BreedId.AddEmptyId();

            Volunteer volunteer = new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);

            Pet pet = new Pet(PetId.AddNewId(), TEST, description, TEST, TEST,
                addres, telephoneNumber, 0, 0, true,
                true, DateTime.Now, DateTime.Now, requisites, [],
                speciesId, breedId, HelpStatus.NeedHelp);

            var addPetResult = volunteer.AddPet(pet);
            var addedPetResult = volunteer.GetPetById(pet.Id);

            Assert.True(addPetResult.IsSuccess);
            Assert.True(addedPetResult.IsSuccess);

            Assert.Equal(addedPetResult.Value.Id, pet.Id);
            Assert.Equal(addedPetResult.Value.SerialNumber, SerialNumber.First);
        }

        [Fact]
        public void Add_Pets_With_Other_Pets_Return_Success_Result()
        {
            const string TEST = "Test";
            const int PETS_COUNT = 10;

            var telephoneNumber = TelephoneNumber.Create("+79482251131").Value;

            var fullName = FullName.Create(
            TEST,
            TEST,
            TEST).Value;

            var description = Description.Create(TEST).Value;
            var experience = Experience.Create(0).Value;

            var socilaNetworkDto = new SocialNetworkDto(TEST, TEST);
            var requisiteDto = new RequisiteDto(TEST, TEST);

            var socialNetworks = new List<SocialNetwork>()
            { SocialNetwork.Create(socilaNetworkDto.name, socilaNetworkDto.reference).Value };

            var requisites = new List<Requisite>()
            { Requisite.Create(requisiteDto.Title, requisiteDto.Description).Value };

            var addres = Addres.Create(
                TEST,
                TEST,
                TEST,
                TEST).Value;

            var speciesId = SpeciesId.AddEmptyId();
            var breedId = BreedId.AddEmptyId();

            Volunteer volunteer = new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);

            var pets = Enumerable.Range(1, PETS_COUNT).Select(p => new Pet(PetId.AddNewId(), TEST, description, TEST, TEST,
                addres, telephoneNumber, 0, 0, true,
                true, DateTime.Now, DateTime.Now, requisites, [],
                speciesId, breedId, HelpStatus.NeedHelp));

            foreach (var pet in pets)
                volunteer.AddPet(pet);

            var petToAdd = new Pet(PetId.AddNewId(), TEST, description, TEST, TEST,
                addres, telephoneNumber, 0, 0, true,
                true, DateTime.Now, DateTime.Now, requisites, [],
                speciesId, breedId, HelpStatus.NeedHelp);

            var addPetResult = volunteer.AddPet(petToAdd);
            var addedPetResult = volunteer.GetPetById(petToAdd.Id);

            var serialNumber = SerialNumber.Create(PETS_COUNT + 1);

            Assert.True(addPetResult.IsSuccess);
            Assert.True(addedPetResult.IsSuccess);

            Assert.Equal(addedPetResult.Value.Id, petToAdd.Id);
            Assert.Equal(addedPetResult.Value.SerialNumber, serialNumber.Value);
            Assert.Equal(addedPetResult.Value.SerialNumber.Value, volunteer.Pets.Count);
        }
    }
}