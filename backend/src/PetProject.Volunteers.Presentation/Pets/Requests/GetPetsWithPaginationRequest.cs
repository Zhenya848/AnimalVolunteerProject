namespace PetProject.Volunteers.Presentation.Pets.Requests
{
    public record GetPetsWithPaginationRequest(
        int Page,
        int PageSize,
        int? PositionFrom = null,
        int? PositionTo = null,
        bool OrderByDesc = false,
        string? OrderBy = null);
}
