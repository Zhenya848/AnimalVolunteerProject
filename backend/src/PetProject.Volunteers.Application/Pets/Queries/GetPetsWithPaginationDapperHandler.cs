using System.Text;
using System.Text.Json;
using Dapper;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.ValueObjects.Dtos;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Volunteers.Application.Pets.Queries
{
    public class GetPetsWithPaginationDapperHandler : IQueryHandler<GetPetsWithPaginationQuery, PagedList<PetDto>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetPetsWithPaginationDapperHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedList<PetDto>> Handle(
            GetPetsWithPaginationQuery query, 
            CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.Create();

            var parameters = new DynamicParameters();

            var sql = new StringBuilder(
                """
                SELECT * FROM pets
                """);

            string paginationForSql = SqlExtensions.ApplyPagination(query.Page, query.PageSize, parameters);
            string sortingForSql = SqlExtensions.ApplySorting(parameters, query.OrderBy, query.OrderByDesc);

            sql.Append(sortingForSql);
            sql.Append(paginationForSql);

            var pets = await connection.QueryAsync<PetDto>(
                sql.ToString(),
                param: parameters);

            var totalCountSql = new StringBuilder("SELECT COUNT(*) FROM pets");

            totalCountSql.Append(sortingForSql);
            totalCountSql.Append(paginationForSql);

            var totalCount = await connection.ExecuteScalarAsync<long>(totalCountSql.ToString());

            return new PagedList<PetDto>()
            {
                Items = pets.ToList(),
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
