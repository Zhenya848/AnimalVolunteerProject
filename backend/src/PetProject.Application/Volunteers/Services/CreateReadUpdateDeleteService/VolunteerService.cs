using CSharpFunctionalExtensions;
using PetProject.Application.Repositories;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Update;
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

            var fullName = FullName.Create(
                request.name.firstName, 
                request.name.lastName, 
                request.name.patronymic ?? "").Value;

            var description = Description.Create(request.description).Value;
            var experience = Experience.Create(request.experience).Value;

            var socialNetworks = request.sotialNetworks
            .Select(s => SocialNetwork.Create(s.name, s.reference).Value).ToList();

            var requisites = request.requisites
            .Select(r => Requisite.Create(r.title, r.description).Value).ToList();

            Volunteer volunteer = new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);

            return await _volunteerRepository.Add(volunteer, cancellationToken);
        }

        public async Task<Result<Guid, Error>> Update(UpdateVolunteerRequest request, 
            CancellationToken cancellationToken = default)
        {
            var volunteer = await _volunteerRepository.GetById(VolunteerId.Create(request.volunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return volunteer.Error;

            var telephoneNumber = TelephoneNumber.Create(request.dto.phoneNumber).Value;
            var fullName = FullName.Create(
                request.dto.name.firstName,
                request.dto.name.lastName,
                request.dto.name.patronymic ?? "").Value;

            var description = Description.Create(request.dto.description).Value;
            var experience = Experience.Create(request.dto.experience).Value;

            var socialNetworks = request.dto.sotialNetworks
            .Select(s => SocialNetwork.Create(s.name, s.reference).Value).ToList();

            var requisites = request.dto.requisites
            .Select(r => Requisite.Create(r.title, r.description).Value).ToList();

            volunteer.Value.UpdateVolunteerInfo(fullName, description, telephoneNumber, experience, socialNetworks, requisites);
            
            return await _volunteerRepository.Save(volunteer.Value, cancellationToken);
        }

        public async Task<Result<Guid, Error>> Delete(DeleteVolunteerRequest request, 
            CancellationToken cancellationToken = default)
        {
            var volunteer = await _volunteerRepository.GetById(VolunteerId.Create(request.volunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return volunteer.Error;

            return await _volunteerRepository.Delete(volunteer.Value);
        }

        public Task<List<Volunteer>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Volunteer> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
