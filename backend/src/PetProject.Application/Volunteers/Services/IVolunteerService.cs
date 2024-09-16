using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Update;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers;

namespace PetProject.Application.Volunteers.Services
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
        Task<Volunteer> GetById(Guid id);
        Task<List<Volunteer>> Get();
    }
}
