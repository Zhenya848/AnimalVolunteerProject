using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Species.Application.Repositories;

namespace PetProject.Species.Application.Species.Commands.Delete
{
    public class DeleteSpeciesHandler : ICommandHandler<DeleteSpeciesCommand, Result<Guid, ErrorList>>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSpeciesHandler(
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext,
            [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork)
        {
            _speciesRepository = speciesRepository;
            _readDbContext = readDbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorList>> Handle(DeleteSpeciesCommand command, CancellationToken cancellationToken = default)
        {
            var speciesResult = await _speciesRepository
                .GetById(SpeciesId.Create(command.id), cancellationToken);

            if (speciesResult.IsFailure)
                return (ErrorList)speciesResult.Error;

            var petsQuery = _readDbContext.Pets;

            if (petsQuery.FirstOrDefault(i => i.SpeciesId == command.id) != null)
                return (ErrorList)Error.Validation("delete.species.error",
                    $"Can't delete species with id {command.id} because this species already used!");

            _speciesRepository.Delete(speciesResult.Value);
            await _unitOfWork.SaveChanges(cancellationToken);

            return command.id;
        }
    }
}
