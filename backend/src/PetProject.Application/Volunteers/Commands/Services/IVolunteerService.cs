using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Application.Volunteers.UseCases.Create;
using PetProject.Application.Volunteers.UseCases.Delete;
using PetProject.Application.Volunteers.UseCases.Update;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers;

namespace PetProject.Application.Volunteers.UseCases.Services
{
    public interface IVolunteerService
    {
        Task<Result<Guid, ErrorList>> Create(CreateVolunteerCommand request, CancellationToken cancellationToken = default);
        Task<Result<Guid, ErrorList>> Delete(
            DeleteVolunteerCommand request,
            CancellationToken cancellationToken = default);
        Task<Result<Guid, ErrorList>> Update(
            UpdateVolunteerCommand request,
            CancellationToken cancellationToken = default);
    }
}
