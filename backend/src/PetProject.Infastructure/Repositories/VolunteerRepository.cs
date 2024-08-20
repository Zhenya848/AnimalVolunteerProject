using PetProject.Domain.Entities.Aggregates;
using PetProject.Application.Repositories;
using PetProject.Domain.ValueObjects.IdClasses;
using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Infastructure.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly AppDbContext _appDbContext;

        public VolunteerRepository(AppDbContext appDbContext) 
            => _appDbContext = appDbContext;

        public async Task<VolunteerId> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            await _appDbContext.Volunteers.AddAsync(volunteer, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id;
        }
    }
}