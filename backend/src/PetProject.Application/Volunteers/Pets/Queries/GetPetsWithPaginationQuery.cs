using PetProject.Application.Shared.Interfaces.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Pets.Queries
{
    public record GetPetsWithPaginationQuery(
        int Page, 
        int PageSize,
        int? PositionFrom = null,
        int? PositionTo = null,
        bool OrderByDesc = false,
        string? OrderBy = null) : IQuery;
}
