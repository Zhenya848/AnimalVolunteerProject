using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Volunteers.Application.Providers;

namespace PetProject.Volunteers.Application.Files.Commands.Create
{
    public class CreateFilesHandler
    {
        private readonly IFileProvider _minioProvider;

        public CreateFilesHandler(IFileProvider minioProvider)
        {
            _minioProvider = minioProvider;
        }

        public async Task<Result<string, Error>> Create(
            CreateFilesCommand request,
            CancellationToken cancellationToken = default)
        {
            var result = await _minioProvider.UploadFiles(request, cancellationToken);

            if (result.IsFailure)
                return result.Error;

            var response = "";
            var files = result.Value;

            for (int i = 0; i < files.Count - 1; i++)
                response += files[i] + ", ";

            response += files[files.Count - 1];

            return response;
        }
    }
}
