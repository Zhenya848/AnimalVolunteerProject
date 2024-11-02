using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Species.Application.Queries
{
    public class GetSpeciesWithPaginationHandler : IQueryHandler<GetSpeciesWithPaginationQuery, PagedList<SpeciesDto>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetSpeciesWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<SpeciesDto>> Handle(
            GetSpeciesWithPaginationQuery query, 
            CancellationToken cancellationToken = default)
        {
            var speciesQuery = _readDbContext.Species;

            return await speciesQuery.GetItemsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
