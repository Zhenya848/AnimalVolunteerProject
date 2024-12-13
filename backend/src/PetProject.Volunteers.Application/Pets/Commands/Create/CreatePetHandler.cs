using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.Application.Messaging;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Volunteers.Application.Volunteers.Repositories;
using PetProject.Volunteers.Domain;
using PetProject.Volunteers.Domain.ValueObjects;
using FileInfo = PetProject.Volunteers.Application.Providers.FileInfo;

namespace PetProject.Volunteers.Application.Pets.Commands.Create
{
    public class CreatePetHandler : ICommandHandler<CreatePetCommand, Result<Guid, ErrorList>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IReadDbContext _readDbContext;

        private readonly IValidator<CreatePetCommand> _createPetValidator;

        public CreatePetHandler(
            IVolunteerRepository volunteerRepository,
            IReadDbContext readDbContext,
            [FromKeyedServices(Modules.Volunteer)]IUnitOfWork unitOfWork,
            IValidator<CreatePetCommand> createPetValidator,
            IMessageQueue<IEnumerable<FileInfo>> messageQueue)
        {
            _volunteerRepository = volunteerRepository;
            _readDbContext = readDbContext;
            _unitOfWork = unitOfWork;
            _createPetValidator = createPetValidator;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
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

            var species = await _readDbContext.Species.Where(i => i.Id == command.SpeciseId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (species == null)
                return (ErrorList)Errors.General.NotFound(command.SpeciseId);

            var breed = await _readDbContext.Breeds.Where(i => i.Id == command.BreedId)
                .FirstOrDefaultAsync(cancellationToken);

            if (breed == null)
                return (ErrorList)Errors.General.NotFound(command.BreedId);

            if (breed.SpeciesId != species.Id)
                return (ErrorList)Errors.General.ValueIsInvalid("Breed");

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
            var addres = Address.Create(
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

            return new Pet(PetId.AddNewId(), command.Name, description, command.Color, command.HealthInfo,
                addres, telephoneNumber, command.Weight, command.Height, command.IsCastrated,
                command.IsVaccinated, command.BirthdayTime, command.DateOfCreation, requisites, [],
                speciesId, breedId, command.HelpStatus);
        }
    }
}
