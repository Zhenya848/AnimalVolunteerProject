using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetProject.Application.Database;
using PetProject.Application.Extensions;
using PetProject.Application.Files.Commands.Create;
using PetProject.Application.Files.Providers;
using PetProject.Application.Messaging;
using PetProject.Application.Repositories.Write;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Application.Volunteers.Pets.Commands.UploadPhotos
{
    public class UploadFilesToPetHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IFileProvider _fileProvider;

        private readonly IValidator<UploadFilesToPetCommand> _uploadFilesValidator;

        private readonly IMessageQueue<IEnumerable<Files.Providers.FileInfo>> _messageQueue;

        public UploadFilesToPetHandler(
            IVolunteerRepository volunteerRepository,
            IFileProvider fileProvider,
            IUnitOfWork unitOfWork,
            IValidator<UploadFilesToPetCommand> uploadFilesValidator,
            IMessageQueue<IEnumerable<Files.Providers.FileInfo>> messageQueue)
        {
            _volunteerRepository = volunteerRepository;
            _fileProvider = fileProvider;
            _unitOfWork = unitOfWork;
            _uploadFilesValidator = uploadFilesValidator;
            _messageQueue = messageQueue;
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
            catch (Exception)
            {
                transaction.Rollback();

                return (ErrorList)Error.Failure("Can not to upload files to pet", "upload.files.failure");
            }
        }
    }
}
