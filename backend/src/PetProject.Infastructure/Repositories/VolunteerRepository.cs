using PetProject.Application.Repositories;
using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using Microsoft.Extensions.Logging;

namespace PetProject.Infastructure.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<VolunteerRepository> _logger;

        public VolunteerRepository(AppDbContext appDbContext, ILogger<VolunteerRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            await _appDbContext.Volunteers.AddAsync(volunteer, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created volunteer {volunteer} with id {volunteer.Id.Value}", volunteer, volunteer.Id.Value);
            return volunteer.Id;
        }

        public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId)
        {
            var volunteer = await _appDbContext.Volunteers
                .FirstOrDefaultAsync(v => v.Id == volunteerId);

            if (volunteer == null)
                return Errors.General.NotFound((Guid)volunteerId);

            return volunteer;
        }

        public async Task<Result<Volunteer, Error>> GetByPhoneNumber(TelephoneNumber phoneNumber)
        {
            var volunteer = await _appDbContext.Volunteers
                .FirstOrDefaultAsync(v => v.TelephoneNumber == phoneNumber);

            if (volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }
    }
}