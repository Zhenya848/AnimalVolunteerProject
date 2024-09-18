using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Species;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;

namespace Tests
{
    public class VolunteersTests
    {
        [Fact]
        public void Add_First_Pet_Init_Return_Success_Result()
        {
            Volunteer volunteer = CreateTestVolunteer();
            Pet pet = CreateTestPetWithName("TEST");

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
            const int PETS_COUNT = 10;

            Volunteer volunteer = CreateTestVolunteer();
            var pets = Enumerable.Range(1, PETS_COUNT).Select(p => CreateTestPetWithName("TEST"));

            foreach (var pet in pets)
                volunteer.AddPet(pet);

            var petToAdd = CreateTestPetWithName("TEST");

            var addPetResult = volunteer.AddPet(petToAdd);
            var addedPetResult = volunteer.GetPetById(petToAdd.Id);

            var serialNumber = SerialNumber.Create(PETS_COUNT + 1);

            Assert.True(addPetResult.IsSuccess);
            Assert.True(addedPetResult.IsSuccess);

            Assert.Equal(addedPetResult.Value.Id, petToAdd.Id);
            Assert.Equal(addedPetResult.Value.SerialNumber, serialNumber.Value);
            Assert.Equal(addedPetResult.Value.SerialNumber.Value, volunteer.Pets.Count);
        }

        [Theory]
        [InlineData(4, 2, "AEBCDFGHIJ")]
        [InlineData(1, 3, "ACBDEFGHIJ")]
        [InlineData(0, 10, "BCDEFGHIJA")]
        [InlineData(9, 1, "JABCDEFGHI")]
        [InlineData(5, 1, "FABCDEGHIJ")]
        [InlineData(6, 10, "ABCDEFHIJG")]
        [InlineData(7, 7, "ABCDEFHGIJ")]
        [InlineData(8, 9, "ABCDEFGHIJ")]
        public void Move_Pets_With_Other_Pets_Return_Success_Result(
            int indexOfPet, 
            int serialNumber, 
            string expectedResult)
        {
            Volunteer volunteer = CreateTestVolunteer();

            volunteer.AddPet(CreateTestPetWithName("A"));
            volunteer.AddPet(CreateTestPetWithName("B"));
            volunteer.AddPet(CreateTestPetWithName("C"));
            volunteer.AddPet(CreateTestPetWithName("D"));
            volunteer.AddPet(CreateTestPetWithName("E"));
            volunteer.AddPet(CreateTestPetWithName("F"));
            volunteer.AddPet(CreateTestPetWithName("G"));
            volunteer.AddPet(CreateTestPetWithName("H"));
            volunteer.AddPet(CreateTestPetWithName("I"));
            volunteer.AddPet(CreateTestPetWithName("J"));

            volunteer.MovePet(volunteer.Pets[indexOfPet], SerialNumber.Create(serialNumber).Value);

            string expectedOrder = "12345678910";

            string actualResult = "";
            string actualOrder = "";

            for (int i = 0; i < volunteer.Pets.Count; i++)
            {
                actualResult += volunteer.Pets[i].Name;
                actualOrder += (int)volunteer.Pets[i].SerialNumber;
            }

            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedOrder, actualOrder);
        }

        private Pet CreateTestPetWithName(string name)
        {
            const string TEST = "Test";

            var description = Description.Create(TEST).Value;

            var addres = Addres.Create(
                TEST,
                TEST,
                TEST,
                TEST).Value;

            var telephoneNumber = TelephoneNumber.Create("+79482251131").Value;

            var speciesId = SpeciesId.AddEmptyId();
            var breedId = BreedId.AddEmptyId();

            var requisiteDto = new RequisiteDto(TEST, TEST);

            var requisites = new List<Requisite>()
            { Requisite.Create(requisiteDto.Title, requisiteDto.Description).Value };

            return new Pet(PetId.AddNewId(), name, description, TEST, TEST,
                addres, telephoneNumber, 0, 0, true,
                true, DateTime.Now, DateTime.Now, requisites, [],
                speciesId, breedId, HelpStatus.NeedHelp);
        }

        private Volunteer CreateTestVolunteer()
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

            return new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);
        }
    }
}