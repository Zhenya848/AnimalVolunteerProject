using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Volunteers.Application.Providers;

namespace PetProject.Volunteers.Application.Files.Commands.Delete
{
    public class DeleteFileHandler
    {
        private readonly IFileProvider _minioProvider;

        public DeleteFileHandler(IFileProvider minioProvider)
        {
            _minioProvider = minioProvider;
        }

        public async Task<Result<string, Error>> Delete(
            DeleteFileCommand request,
            CancellationToken cancellationToken = default)
        {
            return await _minioProvider.DeleteFile(request, cancellationToken);
        }
    }
}
