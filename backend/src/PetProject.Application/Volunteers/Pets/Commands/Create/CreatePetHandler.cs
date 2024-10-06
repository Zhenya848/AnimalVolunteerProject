﻿using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Application.Database;
using PetProject.Application.Extensions;
using PetProject.Application.Messaging;
using PetProject.Application.Repositories.Write;
using PetProject.Application.Shared.Interfaces;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Application.Volunteers.Pets.Commands.Create
{
    public class CreatePetHandler : ICreateHandler<CreatePetCommand, Result<Guid, ErrorList>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;

        private readonly IValidator<CreatePetCommand> _createPetValidator;

        public CreatePetHandler(
            IVolunteerRepository volunteerRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreatePetCommand> createPetValidator,
            IMessageQueue<IEnumerable<Files.Providers.FileInfo>> messageQueue)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _createPetValidator = createPetValidator;
        }

        public async Task<Result<Guid, ErrorList>> Create(
            CreatePetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _createPetValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();

            var volunteer = await _volunteerRepository
                .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteer.IsFailure)
                return (ErrorList)volunteer.Error;

            var pet = InitializePet(command);
            var addPetResult = volunteer.Value.AddPet(pet);

            if (addPetResult.IsFailure)
                return (ErrorList)addPetResult.Error;

            _volunteerRepository.Save(volunteer.Value, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            return (Guid)pet.Id;
        }

        private Pet InitializePet(CreatePetCommand command)
        {
            var addres = Addres.Create(
                command.Addres.Street,
                command.Addres.City,
                command.Addres.State,
                command.Addres.ZipCode).Value;

            var description = Description.Create(command.Description).Value;

            var telephoneNumber = TelephoneNumber.Create(command.TelephoneNumber).Value;

            var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value).ToList();

            var speciesId = SpeciesId.AddEmptyId();
            var breedId = BreedId.AddEmptyId();

            return new Pet(PetId.AddNewId(), command.Name, description, command.Color, command.HealthInfo,
                addres, telephoneNumber, command.Weight, command.Height, command.IsCastrated,
                command.IsVaccinated, command.BirthdayTime, command.DateOfCreation, requisites, [],
                speciesId, breedId, command.HelpStatus);
        }
    }
}