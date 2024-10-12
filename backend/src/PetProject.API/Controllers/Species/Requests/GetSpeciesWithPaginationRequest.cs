namespace PetProject.API.Controllers.Species.Requests
{
    public record GetSpeciesWithPaginationRequest(
        int Page,
        int PageSize);
}
