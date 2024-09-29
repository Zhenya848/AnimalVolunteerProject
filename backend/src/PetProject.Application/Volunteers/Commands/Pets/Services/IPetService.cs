using CSharpFunctionalExtensions;
using PetProject.Application.Volunteers.UseCases.Create;
using PetProject.Application.Volunteers.UseCases.Pets.Create;
using PetProject.Application.Volunteers.UseCases.Pets.UploadPhotos;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.UseCases.Pets.Services
{
    public interface IPetService
    {
        Task<Result<Guid, ErrorList>> Create(
            CreatePetCommand request,
            CancellationToken cancellationToken = default);

        public Task<Result<Guid, ErrorList>> UploadPhotos(
            UploadFilesToPetCommand command,
            CancellationToken cancellationToken = default);
    }
}
