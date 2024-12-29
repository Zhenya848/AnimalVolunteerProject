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
using PetProject.Volunteers.Domain.ValueObjects;

namespace PetProject.Volunteers.Application.Volunteers.Commands.Update
{
    public class UpdateVolunteerHandler : ICommandHandler<UpdateVolunteerCommand, Result<Guid, ErrorList>>
    {
        private readonly IValidator<UpdateVolunteerCommand> _updateValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;

        public UpdateVolunteerHandler(
            IVolunteerRepository volunteerRepository,
            [FromKeyedServices(Modules.Volunteer)]IUnitOfWork unitOfWork,
            IValidator<UpdateVolunteerCommand> updateValidator)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _updateValidator = updateValidator;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
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

            volunteer.Value.UpdateInfo(fullName, description, telephoneNumber, experience);

            _volunteerRepository.Save(volunteer.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            return (Guid)volunteer.Value.Id;
        }
    }
}
