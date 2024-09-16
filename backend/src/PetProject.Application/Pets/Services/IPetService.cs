using CSharpFunctionalExtensions;
using PetProject.Application.Pets.Create;
using PetProject.Application.Pets.UploadPhotos;
using PetProject.Application.Volunteers.Create;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Pets.Services
{
    public interface IPetService
    {
        Task<Result<Guid, ErrorList>> Create(
            CreatePetCommand request, 
            CancellationToken cancellationToken = default);

        public Task<Result<Guid, ErrorList>> UploadFiles(
            UploadFilesToPetCommand command,
            CancellationToken cancellationToken = default);
    }
}
