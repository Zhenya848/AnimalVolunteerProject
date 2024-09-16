using CSharpFunctionalExtensions;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Delete;
using PetProject.Application.Files.Get;
using PetProject.Domain.Shared;

namespace PetProject.Application.Files.Providers
{
    public interface IFileProvider
    {
        public Task<Result<IReadOnlyList<string>, Error>> UploadFiles(
            CreateFilesCommand fileData,
            CancellationToken cancellationToken);

        public Task<Result<string, Error>> DeleteFile(
            DeleteFileCommand fileData,
            CancellationToken cancellationToken);

        public Task<Result<string, Error>> GetFile(
            GetFileCommand fileData,
            CancellationToken cancellationToken);
    }
}
