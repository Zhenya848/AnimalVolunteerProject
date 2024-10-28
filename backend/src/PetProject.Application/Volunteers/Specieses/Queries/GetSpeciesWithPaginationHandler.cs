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

namespace PetProject.Application.Volunteers.Specieses.Queries
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
