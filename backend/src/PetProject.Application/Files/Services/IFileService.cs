using CSharpFunctionalExtensions;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Delete;
using PetProject.Application.Files.Get;
using PetProject.Domain.Shared;

namespace PetProject.Application.Files.Services
{
    public interface IFileService
    {
        public Task<Result<string, Error>> Create(
            CreateFileRequest request,
            CancellationToken cancellationToken = default);

        public Task<Result<string, Error>> Delete(
            DeleteFileRequest request,
            CancellationToken cancellationToken = default);

        public Task<Result<string, Error>> Get(
            GetFileRequest request,
            CancellationToken cancellationToken = default);
    }
}
