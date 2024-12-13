using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.Application.Messaging;
using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Volunteers.Application.Files.Commands.Create;
using PetProject.Volunteers.Application.Providers;
using PetProject.Volunteers.Application.Volunteers.Repositories;
using PetProject.Volunteers.Domain.ValueObjects;
using FileInfo = PetProject.Volunteers.Application.Providers.FileInfo;

namespace PetProject.Volunteers.Application.Pets.Commands.UploadPhotos
{
    public class UploadFilesToPetHandler : ICommandHandler<UploadFilesToPetCommand, Result<Guid, ErrorList>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IFileProvider _fileProvider;

        private readonly IValidator<UploadFilesToPetCommand> _uploadFilesValidator;

        private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

        public UploadFilesToPetHandler(
            IVolunteerRepository volunteerRepository,
            IFileProvider fileProvider,
            [FromKeyedServices(Modules.Volunteer)]IUnitOfWork unitOfWork,
            IValidator<UploadFilesToPetCommand> uploadFilesValidator,
            IMessageQueue<IEnumerable<FileInfo>> messageQueue)
        {
            _volunteerRepository = volunteerRepository;
            _fileProvider = fileProvider;
            _unitOfWork = unitOfWork;
            _uploadFilesValidator = uploadFilesValidator;
            _messageQueue = messageQueue;
        }
        
        public async Task<Result<Guid, ErrorList>> Handle(
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
                    List<FileInfo> filesInfo =
                        files.Select(f => new FileInfo("photos", f.ObjectName)).ToList();

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
