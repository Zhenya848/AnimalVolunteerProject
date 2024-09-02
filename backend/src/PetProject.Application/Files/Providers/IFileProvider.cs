using CSharpFunctionalExtensions;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Delete;
using PetProject.Application.Files.Get;
using PetProject.Domain.Shared;

namespace PetProject.Application.Files.Providers
{
    public interface IFileProvider
    {
        public Task<Result<string, Error>> UploadFile(
            CreateFileRequest fileData,
            CancellationToken cancellationToken);

        public Task<Result<string, Error>> DeleteFile(
            DeleteFileRequest fileData,
            CancellationToken cancellationToken);

        public Task<Result<string, Error>> GetFile(
            GetFileRequest fileData,
            CancellationToken cancellationToken);
    }
}
