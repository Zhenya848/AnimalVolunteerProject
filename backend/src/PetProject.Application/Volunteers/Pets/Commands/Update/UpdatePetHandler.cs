using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Application.Database;
using PetProject.Application.Extensions;
using PetProject.Application.Repositories.Write;
using PetProject.Application.Shared.Interfaces;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Volunteers;
using PetProject.Application.Repositories.Read;
using PetProject.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace PetProject.Application.Volunteers.Pets.Commands.Update
{
    public class UpdatePetHandler : IUpdateHandler<UpdatePetCommand, Result<Guid, ErrorList>>
    {
        private readonly IValidator<UpdatePetCommand> _updateValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IReadDbContext _readDbContext;

        public UpdatePetHandler(
            IVolunteerRepository volunteerRepository,
            IUnitOfWork unitOfWork,
            IValidator<UpdatePetCommand> updateValidator,
            IReadDbContext readDbContext)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _updateValidator = updateValidator;
            _readDbContext = readDbContext;
        }

        public async Task<Result<Guid, ErrorList>> Update(
            UpdatePetCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var volunteer = await _volunteerRepository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return (ErrorList)volunteer.Error;

            var pet = volunteer.Value.GetPetById(PetId.Create(command.PetId));

            if (pet.IsFailure)
                return (ErrorList)pet.Error;

            var species = await _readDbContext.Species.Where(i => i.Id == command.SpeciseId).FirstOrDefaultAsync();

            if (species == null)
                return (ErrorList)Errors.General.NotFound(command.SpeciseId);

            var breed = await _readDbContext.Breeds.Where(i => i.Id == command.BreedId).FirstOrDefaultAsync();

            if (breed == null)
                return (ErrorList)Errors.General.NotFound(command.BreedId);

            if (breed.SpeciesId != species.Id)
                return (ErrorList)Errors.General.ValueIsInvalid("Breed");

            var address = Address.Create(
                command.Addres.Street,
                command.Addres.City,
                command.Addres.State,
                command.Addres.ZipCode).Value;

            var description = Description.Create(command.Description).Value;

            var telephoneNumber = TelephoneNumber.Create(command.TelephoneNumber).Value;

            var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value).ToList();

            var speciesId = SpeciesId.Create(command.SpeciseId);
            var breedId = BreedId.Create(command.BreedId);

            volunteer.Value.UpdatePetInfo(
                command.PetId, command.Name, description, command.Color, command.HealthInfo,
                address, telephoneNumber, command.Weight, command.Height, command.IsCastrated,
                command.IsVaccinated, command.BirthdayTime, command.DateOfCreation, requisites,
                speciesId, breedId, command.HelpStatus);

            _volunteerRepository.Save(volunteer.Value, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            return pet.Value.Id.Value;
        }
    }
}
