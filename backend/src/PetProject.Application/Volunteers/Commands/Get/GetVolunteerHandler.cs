using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.Repositories.Read;
using PetProject.Application.Shared.Interfaces;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Commands.Get
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
