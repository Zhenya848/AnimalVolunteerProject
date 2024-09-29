using Microsoft.EntityFrameworkCore;
using PetProject.Application.Volunteers;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using PetProject.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PetProject.Application.Extensions
{
    public static class QueriesExtensions
    {
        public static async Task<PagedList<T>> GetPetsWithPagination<T>(
            this IQueryable<T> source,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var petsCount = await source.CountAsync(cancellationToken);
            var pets = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<T>()
            {
                Items = pets,
                TotalCount = petsCount,
                Page = page,
                PageSize = pageSize,
            };
        }
    }
}
