using CSharpFunctionalExtensions;
using PetProject.Application.Files.Commands.Create;
using PetProject.Application.Files.Commands.Delete;
using PetProject.Application.Files.Commands.Get;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Application.Files.Providers
{
    public interface IFileProvider
    {
        public Task<Result<IReadOnlyList<string>, Error>> UploadFiles(
            CreateFilesCommand fileData,
            CancellationToken cancellationToken);

        public Result<IReadOnlyList<string>, Error> GetFiles();

        public Task<Result<string, Error>> DeleteFile(
            DeleteFileCommand fileData,
            CancellationToken cancellationToken);

        public Task<Result<string, Error>> GetFile(
            GetFileCommand fileData,
            CancellationToken cancellationToken);
    }
}
