using CSharpFunctionalExtensions;
using PetProject.Application.Database;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Providers;
using PetProject.Application.Repositories;
using PetProject.Application.Volunteers.UseCases.Create;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PetProject.Application.Messaging;
using PetProject.Application.Volunteers.UseCases.Pets.Create;
using PetProject.Application.Volunteers.UseCases.Pets.UploadPhotos;
using PetProject.Application.Extensions;

namespace PetProject.Application.Volunteers.UseCases.Pets.Services
{
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PetService> _logger;

        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IFileProvider _fileProvider;

        private readonly IValidator<CreatePetCommand> _createPetValidator;
        private readonly IValidator<UploadFilesToPetCommand> _uploadFilesValidator;

        private readonly IMessageQueue<IEnumerable<Files.Providers.FileInfo>> _messageQueue;

        public PetService(
            IVolunteerRepository volunteerRepository,
            IFileProvider fileProvider,
            IUnitOfWork unitOfWork,
            ILogger<PetService> logger,
            IValidator<CreatePetCommand> createPetValidator,
            IValidator<UploadFilesToPetCommand> uploadFilesValidator,
            IMessageQueue<IEnumerable<Files.Providers.FileInfo>> messageQueue)
        {
            _volunteerRepository = volunteerRepository;
            _fileProvider = fileProvider;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _createPetValidator = createPetValidator;
            _uploadFilesValidator = uploadFilesValidator;
            _messageQueue = messageQueue;
        }

        public async Task<Result<Guid, ErrorList>> Create(
            CreatePetCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _createPetValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var volunteer = await _volunteerRepository
                .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return (ErrorList)volunteer.Error;

            var pet = InitializePet(command);
            var addPetResult = volunteer.Value.AddPet(pet);

            if (addPetResult.IsFailure)
                return (ErrorList)addPetResult.Error;

            _volunteerRepository.Save(volunteer.Value, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            return (Guid)pet.Id;
        }

        private Pet InitializePet(CreatePetCommand command)
        {
            var addres = Addres.Create(
                command.Addres.Street,
                command.Addres.City,
                command.Addres.State,
                command.Addres.ZipCode).Value;

            var description = Description.Create(command.Description).Value;

            var telephoneNumber = TelephoneNumber.Create(command.TelephoneNumber).Value;

            var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Title, r.Description).Value).ToList();

            var speciesId = SpeciesId.AddEmptyId();
            var breedId = BreedId.AddEmptyId();

            return new Pet(PetId.AddNewId(), command.Name, description, command.Color, command.HealthInfo,
                addres, telephoneNumber, command.Weight, command.Height, command.IsCastrated,
                command.IsVaccinated, command.BirthdayTime, command.DateOfCreation, requisites, [],
                speciesId, breedId, command.HelpStatus);
        }

        public async Task<Result<Guid, ErrorList>> UploadPhotos(
            UploadFilesToPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _uploadFilesValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            try
            {
                var volunteerResult = await _volunteerRepository
                    .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

                if (volunteerResult.IsFailure)
                    return (ErrorList)volunteerResult.Error;

                var petResult = volunteerResult.Value.GetPetById(PetId.Create(command.PetId));

                if (petResult.IsFailure)
                    return (ErrorList)petResult.Error;

                List<PetPhoto> petPhotos = new List<PetPhoto>();
                List<FileData> files = new List<FileData>();

                foreach (var file in command.Files)
                {
                    var pathResult = FilePath.Create(file.FileName);

                    if (pathResult.IsFailure)
                        return (ErrorList)pathResult.Error;

                    var fileData = new FileData(file.Stream, pathResult.Value.FullPath);
                    files.Add(fileData);

                    var petPhoto = PetPhoto.Create(pathResult.Value.FullPath, false).Value;
                    petPhotos.Add(petPhoto);
                }

                petResult.Value.UpdatePhotos(petPhotos);
                await _unitOfWork.SaveChanges(cancellationToken);

                var createFilesCommand = new CreateFilesCommand(files, "photos");
                var uploadResult = await _fileProvider.UploadFiles(createFilesCommand, cancellationToken);

                if (uploadResult.IsFailure)
                {
                    List<Files.Providers.FileInfo> filesInfo =
                        files.Select(f => new Files.Providers.FileInfo("photos", f.ObjectName)).ToList();

                    await _messageQueue.WriteAsync(filesInfo, cancellationToken);

                    return (ErrorList)uploadResult.Error;
                }

                transaction.Commit();

                return petResult.Value.Id.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can not to upload files to pet – {id} in transaction", command.PetId);
                transaction.Rollback();

                return (ErrorList)Error.Failure("Can not to upload files to pet", "upload.files.failure");
            }
        }
    }
}
