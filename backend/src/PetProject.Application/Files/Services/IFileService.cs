using CSharpFunctionalExtensions;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Delete;
using PetProject.Application.Files.Get;
using PetProject.Domain.Shared;

namespace PetProject.Application.Files.Services
{
    public interface IFileService
    {
        public Task<UnitResult<Error>> Create(
            CreateFilesCommand request,
            CancellationToken cancellationToken = default);

        public Task<Result<string, Error>> Delete(
            DeleteFileCommand request,
            CancellationToken cancellationToken = default);

        public Task<Result<string, Error>> Get(
            GetFileCommand request,
            CancellationToken cancellationToken = default);
    }
}
