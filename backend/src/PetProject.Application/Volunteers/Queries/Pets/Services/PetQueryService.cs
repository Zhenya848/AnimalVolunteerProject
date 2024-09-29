using Microsoft.EntityFrameworkCore;
using PetProject.Application.Extensions;
using PetProject.Application.Repositories;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace PetProject.Application.Volunteers.Queries.Pets.Services
{
    public class PetQueryService : IPetQueryService
    {
        private readonly IReadDbContext _readDbContext;

        public PetQueryService(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<PetDto>> Get(
            GetPetsWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var petsQuery = _readDbContext.Pets.AsQueryable();

            return await petsQuery.GetPetsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
