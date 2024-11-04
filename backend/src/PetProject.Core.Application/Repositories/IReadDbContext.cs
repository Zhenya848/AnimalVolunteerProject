using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Core.Application.Repositories
{
    public interface IReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers { get; }
        public IQueryable<PetDto> Pets { get; }

        public IQueryable<SpeciesDto> Species { get; }
        public IQueryable<BreedDto> Breeds { get; }
    }
}
