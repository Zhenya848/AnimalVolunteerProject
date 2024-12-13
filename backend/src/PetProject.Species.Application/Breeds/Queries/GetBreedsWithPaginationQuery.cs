namespace PetProject.Species.Application.Breeds.Queries
{
    public record GetBreedsWithPaginationQuery(
        Guid SpeciesId,
        int Page,
        int PageSize);
}
