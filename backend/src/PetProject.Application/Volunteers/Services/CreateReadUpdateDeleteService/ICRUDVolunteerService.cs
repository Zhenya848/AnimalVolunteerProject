using CSharpFunctionalExtensions;
using PetProject.Application.Volunteers.Create;
using PetProject.Domain.Entities.Aggregates;
using PetProject.Domain.Shared;
using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService
{
    public interface ICRUDVolunteerService
    {
        Task<Result<VolunteerId, Error>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken = default);
        Task Delete(Guid id);
        Task Update(Volunteer entity);
        Task<Volunteer> GetById(Guid id);
        Task<List<Volunteer>> Get();
    }
}
