using CSharpFunctionalExtensions;
using PetProject.Application.Files.Commands.Create;
using PetProject.Application.Files.Commands.Delete;
using PetProject.Application.Files.Commands.Get;
using PetProject.Application.Files.Commands.Update;
using PetProject.Application.Files.Providers;
using PetProject.Application.Shared.Interfaces.Commands;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Commands
{
    public class FileService : ICommandService<Result<string, Error>, CreateFilesCommand, DeleteFileCommand, UpdateFileCommand>
    {
        private readonly IFileProvider _minioProvider;

        public FileService(IFileProvider minioProvider)
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

        public async Task<Result<string, Error>> Delete(
            DeleteFileCommand request,
            CancellationToken cancellationToken = default)
        {
            return await _minioProvider.DeleteFile(request, cancellationToken);
        }

        public async Task<Result<string, Error>> Get(
            GetFileCommand request,
            CancellationToken cancellationToken = default)
        {
            return await _minioProvider.GetFile(request, cancellationToken);
        }

        public Task<Result<string, Error>> Update(UpdateFileCommand request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
