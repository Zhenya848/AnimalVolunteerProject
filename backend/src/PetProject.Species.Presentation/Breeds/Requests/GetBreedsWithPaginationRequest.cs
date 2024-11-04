namespace PetProject.Species.Presentation.Breeds.Requests
{
    public record GetBreedsWithPaginationRequest(
        Guid SpeciesId,
        int Page,
        int PageSize);
}
