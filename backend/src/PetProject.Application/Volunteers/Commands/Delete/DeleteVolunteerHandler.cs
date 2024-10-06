using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Application.Database;
using PetProject.Application.Extensions;
using PetProject.Application.Repositories.Write;
using PetProject.Application.Shared.Interfaces;
using PetProject.Application.Volunteers.UseCases.Delete;
using PetProject.Domain.Shared.ValueObjects.IdClasses;

namespace PetProject.Application.Volunteers.Commands.Delete
{
    public class DeleteVolunteerHandler : IDeleteHandler<DeleteVolunteerCommand, Result<Guid, ErrorList>>
    {
        private readonly IValidator<DeleteVolunteerCommand> _deleteValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;

        public DeleteVolunteerHandler(
            IVolunteerRepository volunteerRepository,
            IUnitOfWork unitOfWork,
            IValidator<DeleteVolunteerCommand> createValidator)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _deleteValidator = createValidator;
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
