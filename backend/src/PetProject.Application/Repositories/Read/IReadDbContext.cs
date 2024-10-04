using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace PetProject.Application.Repositories.Read
{
    public interface IReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers { get; }
        public IQueryable<PetDto> Pets { get; }
    }
}
