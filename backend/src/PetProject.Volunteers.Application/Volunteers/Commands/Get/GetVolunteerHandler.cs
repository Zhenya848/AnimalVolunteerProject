using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Volunteers.Application.Volunteers.Commands.Get
{
    public class GetVolunteerHandler : IQueryHandler<Guid, Result<VolunteerDto, ErrorList>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetVolunteerHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<Result<VolunteerDto, ErrorList>> Handle(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var volunteerQuery = _readDbContext.Volunteers;
            
            var volunteerResult = await volunteerQuery
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

            if (volunteerResult == null)
                return (ErrorList)Errors.General.NotFound(id);

            return volunteerResult;
        }
    }
}
