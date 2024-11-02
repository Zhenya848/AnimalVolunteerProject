using CSharpFunctionalExtensions;
using PetProject.Core;
using PetProject.Core.ValueObjects.IdValueObjects;

namespace PetProject.Species.Application.Repositories
{
    public interface ISpeciesRepository
    {
        Guid Save(Domain.Species species);
        Guid Delete(Domain.Species species);
        Task<Result<Domain.Species, Error>> GetById(
            SpeciesId speciesId,
            CancellationToken cancellationToken = default);
    }
}
