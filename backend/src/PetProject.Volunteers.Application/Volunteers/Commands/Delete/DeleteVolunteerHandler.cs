using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Volunteers.Application.Volunteers.Repositories;

namespace PetProject.Volunteers.Application.Volunteers.Commands.Delete
{
    public class DeleteVolunteerHandler : ICommandHandler<DeleteVolunteerCommand, Result<Guid, ErrorList>>
    {
        private readonly IValidator<DeleteVolunteerCommand> _deleteValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;

        public DeleteVolunteerHandler(
            IVolunteerRepository volunteerRepository,
            [FromKeyedServices(Modules.Volunteer)]IUnitOfWork unitOfWork,
            IValidator<DeleteVolunteerCommand> createValidator)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _deleteValidator = createValidator;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
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
