using PetProject.Application.Extensions;
using PetProject.Application.Repositories.Read;
using PetProject.Application.Shared;
using PetProject.Application.Shared.Interfaces;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System.Linq.Expressions;

namespace PetProject.Application.Volunteers.Pets.Queries
{
    public class GetPetsWithPaginationHandler : IQueryHandler<GetPetsWithPaginationQuery, PagedList<PetDto>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetPetsWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<PetDto>> Get(
            GetPetsWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var petsQuery = _readDbContext.Pets;

            Expression<Func<PetDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "name" => pet => pet.Name,
                "description" => pet => pet.Description,
                "weight" => pet => pet.Weight,
                "height" => pet => pet.Height,
                _ => pet => pet.Id
            };

            petsQuery = query.OrderByDesc
                ? petsQuery.OrderByDescending(selector) : petsQuery.OrderBy(selector);

            petsQuery = petsQuery
                .WhereIf(query.PositionFrom != null, sn => sn.SerialNumber >= query.PositionFrom);

            petsQuery = petsQuery
                .WhereIf(query.PositionTo != null, sn => sn.SerialNumber <= query.PositionTo);

            return await petsQuery.GetPetsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
