using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Queries
{
    public record GetVolunteersWithPaginationQuery(
        int Page,
        int PageSize,
        bool OrderByDesc = false,
        string? OrderBy = null);
}
