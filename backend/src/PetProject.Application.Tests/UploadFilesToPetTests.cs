using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using FluentValidation;
using Moq;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Messaging;
using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.Dtos;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Volunteers.Application.Files.Commands.Create;
using PetProject.Volunteers.Application.Pets.Commands.Create;
using PetProject.Volunteers.Application.Pets.Commands.UploadPhotos;
using PetProject.Volunteers.Application.Providers;
using PetProject.Volunteers.Application.Volunteers.Repositories;
using PetProject.Volunteers.Domain;
using PetProject.Volunteers.Domain.ValueObjects;
using FileInfo = PetProject.Volunteers.Application.Providers.FileInfo;

namespace PetProject.Application.Tests
{
    public class UploadFilesToPetTests
    {
        private readonly Mock<IFileProvider> _fileProviderMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IDbTransaction> _transactionMock = new();
        private readonly Mock<IVolunteerRepository> _volunteerRepository = new();
        private readonly Mock<IMessageQueue<IEnumerable<FileInfo>>> _messageQueue = new();
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

            /*_uploadFilesValidatorMock.Setup(v => v.ValidateAsync(command, ct))
                .ReturnsAsync(new ValidationResult());

            _createPetValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreatePetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());*/
            
            var handler = new UploadFilesToPetHandler(
                _volunteerRepository.Object,
                _fileProviderMock.Object,
                _unitOfWork.Object,
                _uploadFilesValidatorMock.Object,
                _messageQueue.Object);
            // act

            var result = await handler.Handle(command, ct);

            // assert

            Assert.True(result.IsSuccess);
        }

        private Pet CreateTestPet()
        {
            const string TEST = "Test";

            var description = Description.Create(TEST).Value;

            var addres = Address.Create(
                TEST,
                TEST,
                TEST,
                TEST).Value;

            var telephoneNumber = TelephoneNumber.Create("+79482251131").Value;

            var speciesId = SpeciesId.AddEmptyId();
            var breedId = BreedId.AddEmptyId();

            var requisiteDto = new RequisiteDto(TEST, TEST);

            var requisites = new List<Requisite>()
            { Requisite.Create(requisiteDto.Name, requisiteDto.Description).Value };

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
            { Requisite.Create(requisiteDto.Name, requisiteDto.Description).Value };

            return new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);
        }
    }
}
