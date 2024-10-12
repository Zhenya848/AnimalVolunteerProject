using PetProject.Application.Extensions;
using PetProject.Application.Repositories.Read;
using PetProject.Application.Shared;
using PetProject.Application.Shared.Interfaces;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Breeds.Queries
{
    public class GetBreedsWithPaginationHandler : IQueryHandler<GetBreedsWithPaginationQuery, PagedList<BreedDto>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetBreedsWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<BreedDto>> Get(
            GetBreedsWithPaginationQuery query, 
            CancellationToken cancellationToken = default)
        {
            var breedsQuery = _readDbContext.Breeds.Where(si => si.SpeciesId == query.SpeciesId);

            return await breedsQuery.GetItemsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
