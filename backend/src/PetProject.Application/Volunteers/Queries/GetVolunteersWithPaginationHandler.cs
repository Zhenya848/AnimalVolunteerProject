using PetProject.Application.Extensions;
using PetProject.Application.Repositories.Read;
using PetProject.Application.Shared;
using PetProject.Application.Shared.Interfaces;
using PetProject.Application.Volunteers.Pets.Queries;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using PetProject.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Queries
{
    public class GetVolunteersWithPaginationHandler : IQueryHandler<GetVolunteersWithPaginationQuery, PagedList<VolunteerDto>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetVolunteersWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<VolunteerDto>> Get(
            GetVolunteersWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var volunteersQuery = _readDbContext.Volunteers;

            Expression<Func<VolunteerDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "first_name" => volunteer => volunteer.FirstName,
                "last_name" => volunteer => volunteer.LastName,
                "patronymic" => volunteer => volunteer.Patronymic,

                "description" => volunteer => volunteer.Description,
                "experience" => volunteer => volunteer.Experience,
                _ => volunteer => volunteer.Id
            };

            volunteersQuery = query.OrderByDesc 
                ? volunteersQuery.OrderByDescending(selector) : volunteersQuery.OrderBy(selector);

            return await volunteersQuery.GetItemsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
