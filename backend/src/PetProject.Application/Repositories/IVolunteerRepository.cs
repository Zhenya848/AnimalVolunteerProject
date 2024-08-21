using CSharpFunctionalExtensions;
using PetProject.Domain.Entities.Aggregates;
using PetProject.Domain.Shared;
using PetProject.Domain.ValueObjects;
using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.Application.Repositories
{
    public interface IVolunteerRepository
    {
        Task<VolunteerId> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
        Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId);
        Task<Result<Volunteer, Error>> GetByPhoneNumber(TelephoneNumber phoneNumber);
    }
}
