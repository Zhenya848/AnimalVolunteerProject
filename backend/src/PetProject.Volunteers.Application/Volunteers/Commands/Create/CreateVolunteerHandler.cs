using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Volunteers.Application.Volunteers.Repositories;
using PetProject.Volunteers.Domain;
using PetProject.Volunteers.Domain.ValueObjects;

namespace PetProject.Volunteers.Application.Volunteers.Commands.Create
{
    public class CreateVolunteerHandler : ICommandHandler<CreateVolunteerCommand, Result<Guid, ErrorList>>
    {
        private readonly IValidator<CreateVolunteerCommand> _createValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;

        public CreateVolunteerHandler(
            IVolunteerRepository volunteerRepository,
            [FromKeyedServices(Modules.Volunteer)]IUnitOfWork unitOfWork,
            IValidator<CreateVolunteerCommand> createValidator)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
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
