using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Volunteers.Domain;

namespace PetProject.Volunteers.Application.Volunteers.Repositories
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
