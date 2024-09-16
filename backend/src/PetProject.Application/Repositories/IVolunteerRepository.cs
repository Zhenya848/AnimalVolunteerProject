using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Application.Repositories
{
    public interface IVolunteerRepository
    {
        Guid Add(Volunteer volunteer, CancellationToken cancellationToken = default);
        Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default);
        Guid Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
        Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
        Task<Result<Volunteer, Error>> GetByPhoneNumber(TelephoneNumber phoneNumber, CancellationToken cancellationToken = default);
    }
}
