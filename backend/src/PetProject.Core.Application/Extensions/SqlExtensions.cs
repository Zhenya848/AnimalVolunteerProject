using Dapper;

namespace PetProject.Core.Application.Extensions
{
    public static class SqlExtensions
    {
        public static string ApplyPagination(
            int page,
            int pageSize,
            DynamicParameters parameters)
        {
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@Offset", (page - 1) * pageSize);

            return " LIMIT @PageSize OFFSET @Offset";
        }

        public static string ApplySorting(
            DynamicParameters parameters,
            string? OrderBy = null,
            bool OrderByDesc = false)
        {
            string orderDirection = OrderByDesc ? "desc" : "asc";

            parameters.Add("@OrderBy", OrderBy ?? "id");
            parameters.Add("@Direction", orderDirection);

            return " ORDER BY @OrderBy @Direction";
        }
    }
}
