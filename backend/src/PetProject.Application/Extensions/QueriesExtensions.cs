using Microsoft.EntityFrameworkCore;
using PetProject.Application.Shared;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using PetProject.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PetProject.Application.Extensions
{
    public static class QueriesExtensions
    {
        public static async Task<PagedList<T>> GetItemsWithPagination<T>(
            this IQueryable<T> source,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var itemsCount = await source.CountAsync(cancellationToken); 
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<T>()
            {
                Items = items,
                TotalCount = itemsCount,
                Page = page,
                PageSize = pageSize,
            };
        }

        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> source,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
