using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using Microsoft.Extensions.Logging;
using PetProject.Infastructure.DbContexts;
using PetProject.Application.Repositories.Write;

namespace PetProject.Infastructure.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly WriteDbContext _appDbContext;
        private readonly ILogger<VolunteerRepository> _logger;

        public VolunteerRepository(
            WriteDbContext appDbContext, 
            ILogger<VolunteerRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public Guid Add(
            Volunteer volunteer, 
            CancellationToken cancellationToken = default)
        {
            _appDbContext.Volunteers.AddAsync(volunteer, cancellationToken);

            _logger.LogInformation("Created volunteer {volunteer} with id {volunteer.Id.Value}", volunteer, volunteer.Id.Value);
            return volunteer.Id;
        }

        public Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            _appDbContext.Volunteers.Attach(volunteer);

            _logger.LogInformation("Saved volunteer {volunteer} with id {volunteer.Id.Value}", volunteer, volunteer.Id.Value);
            return volunteer.Id;
        }

        public Guid Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            _appDbContext.Volunteers.Remove(volunteer);

            _logger.LogInformation("Removed volunteer {volunteer} with id {volunteer.Id.Value}", volunteer, volunteer.Id.Value);
            return volunteer.Id;
        }

        public async Task<Result<Volunteer, Error>> GetById(
            VolunteerId volunteerId, 
            CancellationToken cancellationToken = default)
        {
            var volunteer = await _appDbContext.Volunteers
                .Include(p => p.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

            if (volunteer == null)
                return Errors.General.NotFound((Guid)volunteerId);

            return volunteer;
        }

        public async Task<Result<Volunteer, Error>> GetByPhoneNumber(
            TelephoneNumber phoneNumber, 
            CancellationToken cancellationToken = default)
        {
            var volunteer = await _appDbContext.Volunteers
                .FirstOrDefaultAsync(v => v.TelephoneNumber == phoneNumber, cancellationToken);

            if (volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }
    }
}