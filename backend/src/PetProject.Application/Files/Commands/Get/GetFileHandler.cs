using CSharpFunctionalExtensions;
using PetProject.Application.Files.Providers;
using PetProject.Domain.Shared;

namespace PetProject.Application.Files.Commands.Get
{
    public class GetFileHandler
    {
        private readonly IFileProvider _minioProvider;

        public GetFileHandler(IFileProvider minioProvider)
        {
            _minioProvider = minioProvider;
        }

        public async Task<Result<string, Error>> Get(
            GetFileCommand request,
            CancellationToken cancellationToken = default)
        {
            return await _minioProvider.GetFile(request, cancellationToken);
        }
    }
}
