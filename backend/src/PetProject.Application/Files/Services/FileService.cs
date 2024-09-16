using CSharpFunctionalExtensions;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Delete;
using PetProject.Application.Files.Get;
using PetProject.Application.Files.Providers;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Services
{
    public class FileService : IFileService
    {
        private readonly IFileProvider _minioProvider;

        public FileService(IFileProvider minioProvider)
        {
            _minioProvider = minioProvider;
        }

        public async Task<UnitResult<Error>> Create(
            CreateFilesCommand request,
            CancellationToken cancellationToken = default)
        {
            return await _minioProvider.UploadFiles(request, cancellationToken);
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
    }
}
