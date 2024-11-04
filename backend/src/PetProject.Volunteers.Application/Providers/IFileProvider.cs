using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Volunteers.Application.Files.Commands.Create;
using PetProject.Volunteers.Application.Files.Commands.Delete;
using PetProject.Volunteers.Application.Files.Commands.Get;

namespace PetProject.Volunteers.Application.Providers
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
