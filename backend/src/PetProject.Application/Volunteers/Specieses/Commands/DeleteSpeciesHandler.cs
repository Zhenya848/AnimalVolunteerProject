using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Application.Database;
using PetProject.Application.Repositories.Read;
using PetProject.Application.Repositories.Write;
using PetProject.Application.Shared.Interfaces;
using PetProject.Application.Volunteers.UseCases.Delete;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Specieses.Commands
{
    public class DeleteSpeciesHandler : IDeleteHandler<DeleteSpeciesCommand, Result<Guid, ErrorList>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;

        public DeleteSpeciesHandler(
            ISpeciesRepository speciesRepository,
            IUnitOfWork unitOfWork,
            IReadDbContext readDbContext)
        {
            _speciesRepository = speciesRepository;
            _unitOfWork = unitOfWork;
            _readDbContext = readDbContext;
        }

        public async Task<Result<Guid, ErrorList>> Delete(DeleteSpeciesCommand command, CancellationToken cancellationToken = default)
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
