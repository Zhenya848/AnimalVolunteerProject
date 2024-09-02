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
            var telephoneNumber = TelephoneNumber.Create(request.PhoneNumber).Value;
            var existVolunteer = await _volunteerRepository.GetByPhoneNumber(telephoneNumber);

            if (existVolunteer.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var fullName = FullName.Create(
                request.Name.firstName, 
                request.Name.lastName, 
                request.Name.patronymic ?? "").Value;

            var description = Description.Create(request.Description).Value;
            var experience = Experience.Create(request.Experience).Value;

            var socialNetworks = request.SotialNetworks
            .Select(s => SocialNetwork.Create(s.name, s.reference).Value).ToList();

            var requisites = request.Requisites
            .Select(r => Requisite.Create(r.title, r.description).Value).ToList();

            Volunteer volunteer = new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);

            return await _volunteerRepository.Add(volunteer, cancellationToken);
        }

        public async Task<Result<Guid, Error>> Update(UpdateVolunteerRequest request, 
            CancellationToken cancellationToken = default)
        {
            var volunteer = await _volunteerRepository.GetById(VolunteerId.Create(request.VolunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return volunteer.Error;

            var telephoneNumber = TelephoneNumber.Create(request.Dto.PhoneNumber).Value;
            var fullName = FullName.Create(
                request.Dto.Name.firstName,
                request.Dto.Name.lastName,
                request.Dto.Name.patronymic ?? "").Value;

            var description = Description.Create(request.Dto.Description).Value;
            var experience = Experience.Create(request.Dto.Experience).Value;

            var socialNetworks = request.Dto.SotialNetworks
            .Select(s => SocialNetwork.Create(s.name, s.reference).Value).ToList();

            var requisites = request.Dto.Requisites
            .Select(r => Requisite.Create(r.title, r.description).Value).ToList();

            volunteer.Value.UpdateInfo(fullName, description, telephoneNumber, experience, socialNetworks, requisites);
            
            return await _volunteerRepository.Save(volunteer.Value, cancellationToken);
        }

        public async Task<Result<Guid, Error>> Delete(DeleteVolunteerRequest request, 
            CancellationToken cancellationToken = default)
        {
            var volunteer = await _volunteerRepository.GetById(VolunteerId.Create(request.VolunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return volunteer.Error;

            volunteer.Value.Delete();
            return await _volunteerRepository.Save(volunteer.Value);
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
