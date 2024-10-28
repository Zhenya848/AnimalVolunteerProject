using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Application.Database;
using PetProject.Application.Extensions;
using PetProject.Application.Repositories.Write;
using PetProject.Application.Shared.Interfaces;
using PetProject.Application.Volunteers.UseCases.Delete;
using PetProject.Application.Volunteers.UseCases.Update;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Commands.Update
{
    public class UpdateVolunteerHandler : ICommandHandler<UpdateVolunteerCommand, Result<Guid, ErrorList>>
    {
        private readonly IValidator<UpdateVolunteerCommand> _updateValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;

        public UpdateVolunteerHandler(
            IVolunteerRepository volunteerRepository,
            IUnitOfWork unitOfWork,
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

            var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Name, s.Reference).Value).ToList();

            var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value).ToList();

            volunteer.Value.UpdateInfo(fullName, description, telephoneNumber, experience, socialNetworks, requisites);

            _volunteerRepository.Save(volunteer.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            return (Guid)volunteer.Value.Id;
        }
    }
}
