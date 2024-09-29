using PetProject.Application.Repositories;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Queries.Pets.Services
{
    public interface IPetQueryService
    {
        public Task<PagedList<PetDto>> Get(
            GetPetsWithPaginationQuery query,
            CancellationToken cancellationToken = default);
    }
}
