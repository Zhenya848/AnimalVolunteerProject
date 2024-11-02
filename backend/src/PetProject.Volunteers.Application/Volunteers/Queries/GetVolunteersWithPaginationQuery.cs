namespace PetProject.Volunteers.Application.Volunteers.Queries
{
    public record GetVolunteersWithPaginationQuery(
        int Page,
        int PageSize,
        bool OrderByDesc = false,
        string? OrderBy = null);
}
