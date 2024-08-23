using CSharpFunctionalExtensions;
using PetProject.Application.Repositories;
using PetProject.Application.Volunteers.Create;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService
{
    public class VolunteerService : IVolunteerService
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public VolunteerService(IVolunteerRepository volunteerRepository)
        {
            _volunteerRepository = volunteerRepository;
        }

        public async Task<Result<Guid, Error>> Create(CreateVolunteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var telephoneNumber = TelephoneNumber.Create(request.phoneNumber).Value;
            var existVolunteer = await _volunteerRepository.GetByPhoneNumber(telephoneNumber);

            if (existVolunteer.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var fullName = FullName.Create(request.firstname, request.lastName, request.patronymic ?? "").Value;
            var description = Description.Create(request.description).Value;
            var exp = Experience.Create(request.exp).Value;

            var socialNetworks = request.sotialNetworks
            .Select(s => SocialNetwork.Create(s.name, s.reference).Value).ToList();

            var requisites = request.requisites
            .Select(r => Requisite.Create(r.title, r.description).Value).ToList();

            Volunteer volunteer = new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, exp, socialNetworks, requisites);

            return await _volunteerRepository.Add(volunteer, cancellationToken);
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
