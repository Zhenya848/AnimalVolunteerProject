using CSharpFunctionalExtensions;
using FluentValidation;
using Moq;
using PetProject.Application.Database;
using PetProject.Application.Files.Providers;
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
using Microsoft.Extensions.Logging;
using System.Data;
using PetProject.Application.Volunteers.Pets.Commands.UploadPhotos;
using PetProject.Application.Volunteers.Pets.Commands.Create;
using PetProject.Application.Volunteers.Pets.Commands;
using PetProject.Application.Files.Commands.Create;
using PetProject.Application.Repositories.Write;

namespace PetProject.Application.Tests
{
    public class UploadFilesToPetTests
    {
        private readonly Mock<IFileProvider> _fileProviderMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IDbTransaction> _transactionMock = new();
        private readonly Mock<IVolunteerRepository> _volunteerRepository = new();
        private readonly Mock<ILogger<PetService>> _loggerMock = new();
        private readonly Mock<IValidator<UploadFilesToPetCommand>> _uploadFilesValidatorMock = new();
        private readonly Mock<IValidator<CreatePetCommand>> _createPetValidatorMock = new();

        [Fact]
        public async void Service_Should_Upload_Files_To_Pet()
        {
            // arrange

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

            _fileProviderMock.Setup(u => u.UploadFiles(createFilesCommand, ct))
                .ReturnsAsync(Result.Success<IReadOnlyList<string>, Error>(photoPaths));

            _unitOfWork.Setup(s => s.SaveChanges(ct))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(c => c.BeginTransaction(ct)).ReturnsAsync(_transactionMock.Object);

            _volunteerRepository.Setup(g => g.GetById(volunteer.Id, ct))
                .ReturnsAsync(volunteer);

            _uploadFilesValidatorMock.Setup(v => v.ValidateAsync(command, ct))
                .ReturnsAsync(new ValidationResult());

            _createPetValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreatePetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());

            var petService = new PetService(
                _volunteerRepository.Object,
                _fileProviderMock.Object,
                _unitOfWork.Object,
                _loggerMock.Object,
                _createPetValidatorMock.Object,
                _uploadFilesValidatorMock.Object);

            // act

            var result = await petService.UploadPhotos(command, ct);

            // assert

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
            { SocialNetwork.Create(socilaNetworkDto.Name, socilaNetworkDto.Reference).Value };

            var requisites = new List<Requisite>()
            { Requisite.Create(requisiteDto.Title, requisiteDto.Description).Value };

            return new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);
        }
    }
}
