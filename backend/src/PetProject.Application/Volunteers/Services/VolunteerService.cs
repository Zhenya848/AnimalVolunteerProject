using CSharpFunctionalExtensions;
using PetProject.Application.Repositories;
using PetProject.Application.Volunteers.Create;
using PetProject.Domain.Entities.Aggregates;
using PetProject.Domain.Shared;
using PetProject.Domain.ValueObjects;
using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.Application.Volunteers.Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public VolunteerService(IVolunteerRepository volunteerRepository) =>
            _volunteerRepository = volunteerRepository;

        public async Task<Result<VolunteerId, Error>> Create(CreateVolunteerRequest createVolunteerRequest,
            CancellationToken cancellationToken = default)
        {
            List<SocialNetwork> socialNetworks = createVolunteerRequest.sotialNetworks
            .Select(s => SocialNetwork.Create(s.name, s.reference).Value).ToList();

            List<Requisite> requesites = createVolunteerRequest.requisites
            .Select(r => Requisite.Create(r.title, r.description).Value).ToList();

            var volunteer = Volunteer.Create(VolunteerId.AddNewId(), createVolunteerRequest.firstname,
                createVolunteerRequest.lastName, createVolunteerRequest.patronymic, createVolunteerRequest.description,
                createVolunteerRequest.phoneNumber, createVolunteerRequest.exp, socialNetworks,
                requesites, []);

            if (volunteer.IsFailure)
                return Errors.General.ValueIsInvalid("Create volunteer fail!");

            return await _volunteerRepository.Add(volunteer.Value, cancellationToken);
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Volunteer>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Volunteer> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Volunteer entity)
        {
            throw new NotImplementedException();
        }
    }
}
