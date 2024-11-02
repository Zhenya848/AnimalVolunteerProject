namespace PetProject.Species.Application.Queries
{
    public record GetBreedsWithPaginationQuery(
        Guid SpeciesId,
        int Page,
        int PageSize);
}
