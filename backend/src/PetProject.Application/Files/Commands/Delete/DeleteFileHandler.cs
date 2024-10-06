using CSharpFunctionalExtensions;
using PetProject.Application.Files.Providers;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Commands.Delete
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
