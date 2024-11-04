namespace PetProject.Species.Application.Queries
{
    public record GetSpeciesWithPaginationQuery(
        int Page,
        int PageSize);
}
