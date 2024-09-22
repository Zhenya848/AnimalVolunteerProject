using CSharpFunctionalExtensions;
using FluentValidation;
using Moq;
using PetProject.Application.Database;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Providers;
using PetProject.Application.Pets.UploadPhotos;
using PetProject.Application.Repositories;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetProject.Application.Pets.Create;
using Microsoft.Extensions.Logging;
using PetProject.Application.Pets.Services;
using System.Data;

namespace PetProject.Application.Tests
{
    public class UploadFilesToPetTests
    {
        [Fact]
        public async void Service_Should_Upload_Files_To_Pet()
        {
            var ct = new CancellationTokenSource().Token;

            var volunteer = CreateTestVolunteer();
            var pet = CreateTestPet();

            volunteer.AddPet(pet);

            var uploadFileDto = new UploadFileDto("Test.jpg", "", new MemoryStream());

            List<UploadFileDto> filesDto = new List<UploadFileDto>() 
                { uploadFileDto, uploadFileDto, uploadFileDto };

            var command = new UploadFilesToPetCommand(volunteer.Id, pet.Id, filesDto);

            List<FileData> files = new List<FileData>();
            List<PetPhoto> petPhotos = new List<PetPhoto>();

            foreach (var file in command.Files)
            {
                var pathResult = FilePath.Create(file.FileName);

                var fileData = new FileData(file.Stream, pathResult.Value.FullPath);
                files.Add(fileData);

                var petPhoto = PetPhoto.Create(pathResult.Value.FullPath, false).Value;
                petPhotos.Add(petPhoto);
            }

            var createFilesCommand = new CreateFilesCommand(files, "photos");

            List<string> photoPaths = 
                createFilesCommand.Files.Select(p => p.ObjectName).ToList();

            var fileProviderMock = new Mock<IFileProvider>();

            fileProviderMock.Setup(u => u.UploadFiles(createFilesCommand, ct))
                .ReturnsAsync(Result.Success<IReadOnlyList<string>, Error>(photoPaths));

            var unitOfWork = new Mock<IUnitOfWork>();

            unitOfWork.Setup(s => s.SaveChanges(ct))
                .Returns(Task.CompletedTask);

            var transactionMock = new Mock<IDbTransaction>();

            unitOfWork.Setup(c => c.BeginTransaction(ct)).ReturnsAsync(transactionMock.Object);

            var volunteerRepository = new Mock<IVolunteerRepository>();

            volunteerRepository.Setup(g => g.GetById(volunteer.Id, ct))
                .ReturnsAsync(volunteer);

            var uploadFilesValidatorMock = new Mock<IValidator<UploadFilesToPetCommand>>();

            uploadFilesValidatorMock.Setup(v => v.ValidateAsync(command, ct))
                .ReturnsAsync(new ValidationResult());

            var createPetValidatorMock = new Mock<IValidator<CreatePetCommand>>();

            createPetValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreatePetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());

            var logger = LoggerFactory.Create(builder => builder.AddConsole())
                .CreateLogger<PetService>();

            var petService = new PetService(
                volunteerRepository.Object,
                fileProviderMock.Object,
                unitOfWork.Object,
                logger,
                createPetValidatorMock.Object,
                uploadFilesValidatorMock.Object);

            var result = await petService.UploadPhotos(command, ct);

            Assert.True(result.IsSuccess);
        }

        private Pet CreateTestPet()
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

            return new Pet(PetId.AddNewId(), TEST, description, TEST, TEST,
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
