using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Volunteers.Application.Providers;

namespace PetProject.Volunteers.Application.Files.Commands.Get
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
