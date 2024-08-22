using CSharpFunctionalExtensions;
using PetProject.Application.Volunteers.Create;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers;

namespace PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService
{
    public interface IVolunteerService
    {
        Task<Result<Guid, Error>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken = default);
        Task Delete(Guid id);
        Task Update(Volunteer entity);
        Task<Volunteer> GetById(Guid id);
        Task<List<Volunteer>> Get();
    }
}
