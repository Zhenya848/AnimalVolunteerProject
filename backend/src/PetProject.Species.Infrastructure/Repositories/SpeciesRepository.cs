using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.Core;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Species.Application.Repositories;
using PetProject.Species.Infrastructure.DbContexts;

namespace PetProject.Species.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly WriteDbContext _appDbContext;
        private readonly ILogger<SpeciesRepository> _logger;

        public SpeciesRepository(
            WriteDbContext appDbContext,
            ILogger<SpeciesRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public Guid Save(Domain.Species species)
        {
            _appDbContext.Species.Attach(species);

            _logger.LogInformation("Saved species {species} with id {species.Id.Value}", species, species.Id.Value);
            return species.Id.Value;
        }

        public Guid Delete(Domain.Species species)
        {
            _appDbContext.Species.Remove(species);

            _logger.LogInformation("Removed species {species} with id {species.Id.Value}", species, species.Id.Value);
            return species.Id.Value;
        }

        public async Task<Result<Domain.Species, Error>> GetById(
            SpeciesId speciesId,
            CancellationToken cancellationToken = default)
        {
            var species = await _appDbContext.Species
                .Include(b => b.Breeds)
                .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

            if (species == null)
                return Errors.General.NotFound(speciesId.Value);

            return species;
        }
    }
}
