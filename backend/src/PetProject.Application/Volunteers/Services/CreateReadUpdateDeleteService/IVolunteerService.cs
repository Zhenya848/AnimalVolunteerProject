using CSharpFunctionalExtensions;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Update;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers;

namespace PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService
{
    public interface IVolunteerService
    {
        Task<Result<Guid, Error>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken = default);
        Task<Result<Guid, Error>> Delete(DeleteVolunteerRequest request, CancellationToken cancellationToken = default);
        Task<Result<Guid, Error>> Update(UpdateVolunteerRequest request, CancellationToken cancellationToken = default);
        Task<Volunteer> GetById(Guid id);
        Task<List<Volunteer>> Get();
    }
}
