namespace PetProject.API.Controllers.Breeds.Requests
{
    public record GetBreedsWithPaginationRequest(
        Guid SpeciesId,
        int Page,
        int PageSize);
}
