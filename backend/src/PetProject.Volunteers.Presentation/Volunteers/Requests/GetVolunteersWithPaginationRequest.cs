namespace PetProject.Volunteers.Presentation.Volunteers.Requests
{
    public record GetVolunteersWithPaginationRequest(
        int Page,
        int PageSize,
        bool OrderByDesc = false,
        string? OrderBy = null);
}
