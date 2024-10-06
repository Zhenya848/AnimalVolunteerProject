using CSharpFunctionalExtensions;
using PetProject.Application.Database;
using PetProject.Application.Repositories.Write;
using PetProject.Application.Shared.Interfaces;
using PetProject.Application.Volunteers.UseCases.Create;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Volunteers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetProject.Application.Extensions;

namespace PetProject.Application.Volunteers.Commands.Create
{
    public class CreateVolunteerHandler : ICreateHandler<CreateVolunteerCommand, Result<Guid, ErrorList>>
    {
        private readonly IValidator<CreateVolunteerCommand> _createValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;

        public CreateVolunteerHandler(
            IVolunteerRepository volunteerRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreateVolunteerCommand> createValidator)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
        }

        public async Task<Result<Guid, ErrorList>> Create(
            CreateVolunteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var telephoneNumber = TelephoneNumber.Create(command.PhoneNumber).Value;
            var existVolunteer = await _volunteerRepository.GetByPhoneNumber(telephoneNumber);

            if (existVolunteer.IsSuccess)
                return (ErrorList)Errors.Volunteer.AlreadyExist();

            var fullName = FullName.Create(
                command.Name.firstName,
                command.Name.lastName,
                command.Name.patronymic ?? "").Value;

            var description = Description.Create(command.Description).Value;
            var experience = Experience.Create(command.Experience).Value;

            var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Name, s.Reference).Value).ToList();

            var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value).ToList();

            Volunteer volunteer = new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);

            _volunteerRepository.Add(volunteer);

            await _unitOfWork.SaveChanges(cancellationToken);

            return (Guid)volunteer.Id;
        }
    }
}
