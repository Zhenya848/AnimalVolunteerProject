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

            var speciesQuery = _readDbContext.Species;
            var breedsQuery = _readDbContext.Breeds;

            Expression<Func<PetDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "volunteerId" => pet => pet.VolunteerId,
                "name" => pet => pet.Name,
                "color" => pet => pet.Color,
                "species" => pet => speciesQuery.Where(i => i.Id == pet.Id).First(),
                "breeds" => pet => breedsQuery.Where(i => i.Id == pet.Id).First(),
                "description" => pet => pet.Description,
                "weight" => pet => pet.Weight,
                "height" => pet => pet.Height,
                "street" => pet => pet.Street,
                "city" => pet => pet.City,
                "state" => pet => pet.State,
                "zipcode" => pet => pet.Zipcode,

                _ => pet => pet.Id
            };

            /*
               public string Street { get; }
               public string City { get; }
               public string State { get; }
               public string ZipCode { get; }*/

            petsQuery = query.OrderByDesc
                ? petsQuery.OrderByDescending(selector) : petsQuery.OrderBy(selector);

            petsQuery = petsQuery
                .WhereIf(query.PositionFrom != null, sn => sn.SerialNumber >= query.PositionFrom);

            petsQuery = petsQuery
                .WhereIf(query.PositionTo != null, sn => sn.SerialNumber <= query.PositionTo);

            return await petsQuery.GetItemsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
