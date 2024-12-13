using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Species.Application.Breeds.Queries
{
    public class GetBreedsWithPaginationHandler : IQueryHandler<GetBreedsWithPaginationQuery, PagedList<BreedDto>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetBreedsWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<BreedDto>> Handle(
            GetBreedsWithPaginationQuery query, 
            CancellationToken cancellationToken = default)
        {
            var breedsQuery = _readDbContext.Breeds.Where(si => si.SpeciesId == query.SpeciesId);

            return await breedsQuery.GetItemsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
