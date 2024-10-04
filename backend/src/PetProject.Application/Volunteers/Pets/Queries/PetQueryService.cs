using Dapper;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.Extensions;
using PetProject.Application.Repositories.Read;
using PetProject.Application.Shared;
using PetProject.Application.Shared.Interfaces.Queries;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using PetProject.Domain.Volunteers;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace PetProject.Application.Volunteers.Pets.Queries
{
    public class PetQueryService : IQueryService<PagedList<PetDto>, GetPetsWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly ISqlConnectionFactory _connectionFactory;

        public PetQueryService(IReadDbContext readDbContext, ISqlConnectionFactory connectionFactory)
        {
            _readDbContext = readDbContext;
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedList<PetDto>> Get(
            GetPetsWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var petsQuery = _readDbContext.Pets;

            Expression<Func<PetDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "name" => pet => pet.Name,
                "description" => pet => pet.Description,
                "weight" => pet => pet.Weight,
                "height" => pet => pet.Height,
                _ => pet => pet.Id
            };

            petsQuery = query.OrderByDesc
                ? petsQuery.OrderByDescending(selector) : petsQuery.OrderBy(selector);

            petsQuery = petsQuery
                .WhereIf(query.PositionFrom != null, sn => sn.SerialNumber >= query.PositionFrom);

            petsQuery = petsQuery
                .WhereIf(query.PositionTo != null, sn => sn.SerialNumber <= query.PositionTo);

            return await petsQuery.GetPetsWithPagination(query.Page, query.PageSize, cancellationToken);
        }

        public async Task<PagedList<PetDto>> GetWithDapper(
            GetPetsWithPaginationQuery query)
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

            var pets = await connection.QueryAsync<PetDto, string, PetDto>(
                sql.ToString(), 
                (pets, jsonRequisites) =>
                {
                    var requisites = JsonSerializer.Deserialize<RequisiteDto[]>(jsonRequisites) ?? [];

                    pets.Requisites = requisites;

                    return pets;
                },
                splitOn: "Requisites",
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