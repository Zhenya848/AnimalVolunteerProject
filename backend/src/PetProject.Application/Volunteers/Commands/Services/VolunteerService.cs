using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using PetProject.Application.Database;
using PetProject.Application.Volunteers.UseCases.Pets.Services;
using PetProject.Application.Repositories;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;
using System.ComponentModel.DataAnnotations;
using PetProject.Application.Volunteers.UseCases.Update;
using PetProject.Application.Volunteers.UseCases.Create;
using PetProject.Application.Volunteers.UseCases.Delete;
using PetProject.Application.Extensions;

namespace PetProject.Application.Volunteers.UseCases.Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VolunteerService> _logger;
        private readonly IVolunteerRepository _volunteerRepository;

        private readonly IValidator<CreateVolunteerCommand> _createValidator;
        private readonly IValidator<UpdateVolunteerCommand> _updateValidator;
        private readonly IValidator<DeleteVolunteerCommand> _deleteValidator;

        public VolunteerService(
            IVolunteerRepository volunteerRepository,
            IUnitOfWork unitOfWork,
            ILogger<VolunteerService> logger,
            IValidator<CreateVolunteerCommand> createValidator,
            IValidator<UpdateVolunteerCommand> updateValidator,
            IValidator<DeleteVolunteerCommand> deleteValidator)
        {
            _volunteerRepository = volunteerRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;
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
            .Select(s => SocialNetwork.Create(s.name, s.reference).Value).ToList();

            var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Title, r.Description).Value).ToList();

            Volunteer volunteer = new Volunteer(VolunteerId.AddNewId(), fullName, description,
                telephoneNumber, experience, socialNetworks, requisites);

            _volunteerRepository.Add(volunteer);

            await _unitOfWork.SaveChanges(cancellationToken);

            return (Guid)volunteer.Id;
        }

        public async Task<Result<Guid, ErrorList>> Update(
            UpdateVolunteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var volunteer = await _volunteerRepository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return (ErrorList)volunteer.Error;

            var telephoneNumber = TelephoneNumber.Create(command.PhoneNumber).Value;
            var fullName = FullName.Create(
                command.Name.firstName,
                command.Name.lastName,
                command.Name.patronymic ?? "").Value;

            var description = Description.Create(command.Description).Value;
            var experience = Experience.Create(command.Experience).Value;

            var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.name, s.reference).Value).ToList();

            var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Title, r.Description).Value).ToList();

            volunteer.Value.UpdateInfo(fullName, description, telephoneNumber, experience, socialNetworks, requisites);

            _volunteerRepository.Save(volunteer.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            return (Guid)volunteer.Value.Id;
        }

        public async Task<Result<Guid, ErrorList>> Delete(
            DeleteVolunteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _deleteValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var volunteer = await _volunteerRepository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return (ErrorList)volunteer.Error;

            volunteer.Value.Delete();

            _volunteerRepository.Save(volunteer.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            return (Guid)volunteer.Value.Id;
        }
    }
}
