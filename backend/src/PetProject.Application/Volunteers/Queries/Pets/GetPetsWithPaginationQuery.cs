using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Volunteers.Queries.Pets
{
    public record GetPetsWithPaginationQuery(int Page, int PageSize);
}
