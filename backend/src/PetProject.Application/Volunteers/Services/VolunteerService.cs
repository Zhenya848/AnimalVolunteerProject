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

        public async Task<Result<VolunteerId, Error>> Create(CreateVolunteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var fullName = FullName.Create(request.firstname, request.lastName, request.patronymic);

            if (fullName.IsFailure)
                return fullName.Error;

            var telephoneNumber = TelephoneNumber.Create(request.phoneNumber);

            if (telephoneNumber.IsFailure)
                return telephoneNumber.Error;

            var socialNetworks = request.sotialNetworks
            .Select(s => SocialNetwork.Create(s.name, s.reference)).ToList();

            if (socialNetworks.Any(s => s.IsFailure))
                return Errors.General.ValueIsInvalid("One or more links to a social network");

            var requisites = request.requisites
            .Select(r => Requisite.Create(r.title, r.description)).ToList();

            if (requisites.Any(r => r.IsFailure))
                return Errors.General.ValueIsInvalid("One or more requisites");

            if (string.IsNullOrWhiteSpace(request.description))
                return Errors.General.ValueIsInvalid("Description is null or white space!");

            if (request.exp < 0)
                return Errors.General.ValueIsInvalid("Exp can't be less than zero!");

            var volunteer = Volunteer.Create(VolunteerId.AddNewId(), fullName.Value, request.description,
                telephoneNumber.Value, request.exp, socialNetworks.Select(s => s.Value).ToList(),
                requisites.Select(r => r.Value).ToList(), []);

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
