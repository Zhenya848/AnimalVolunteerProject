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
            Assert.Equal(addedPetResult.Value.SerialNumber!.Value, volunteer.Pets.Count);
        }

        [Fact]
        public void Move_Pets_With_Other_Pets_Return_Success_Result()
        {
            Volunteer volunteer = CreateTestVolunteer();
            const string RIGHT_ORDER = "12345678910";

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

            var test1 = CheckPetMove(
                volunteer,
                volunteer.Pets[4],
                SerialNumber.Create(2).Value,
                "AEBCDFGHIJ",
                RIGHT_ORDER);

            var test2 = CheckPetMove(
                volunteer,
                volunteer.Pets[0],
                SerialNumber.Create(10).Value,
                "EBCDFGHIJA",
                RIGHT_ORDER);

            var test3 = CheckPetMove(
                volunteer,
                volunteer.Pets[9],
                SerialNumber.Create(1).Value,
                "AEBCDFGHIJ",
                RIGHT_ORDER);

            var test4 = CheckPetMove(
                volunteer,
                volunteer.Pets[5],
                SerialNumber.Create(1).Value,
                "FAEBCDGHIJ",
                RIGHT_ORDER);

            var test5 = CheckPetMove(
                volunteer,
                volunteer.Pets[6],
                SerialNumber.Create(9).Value,
                "FAEBCDHIGJ",
                RIGHT_ORDER);

            var test6 = CheckPetMove(
                volunteer,
                volunteer.Pets[1],
                SerialNumber.Create(3).Value,
                "FEABCDHIGJ",
                RIGHT_ORDER);

            var test7 = CheckPetMove(
                volunteer,
                volunteer.Pets[6],
                SerialNumber.Create(6).Value,
                "FEABCHDIGJ",
                RIGHT_ORDER);

            var test8 = CheckPetMove(
                volunteer,
                volunteer.Pets[6],
                SerialNumber.Create(7).Value,
                "FEABCHDIGJ",
                RIGHT_ORDER);

            List<(string petNames, string rightAnswer, string order, string rightOrder)> tests =
                [test1, test2, test3, test4, test5, test6, test7, test8];

            foreach (var test in tests)
            {
                Assert.Equal(test.rightAnswer, test.petNames);
                Assert.Equal(test.rightOrder, test.order);
            }
        }

        private (string petNames, string rightAnswer, string order, string rightOrder) CheckPetMove(
            Volunteer volunteer,
            Pet pet,
            SerialNumber serialNumber,
            string expectedResult,
            string expectedOrder)
        {
            volunteer.MovePet(pet, serialNumber);

            string petNames = "";
            string order = "";

            foreach (var current in volunteer.Pets)
            {
                petNames += current.Name;
                order += current.SerialNumber!.Value.ToString();
            }

            return (petNames, expectedResult, order, expectedOrder);
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