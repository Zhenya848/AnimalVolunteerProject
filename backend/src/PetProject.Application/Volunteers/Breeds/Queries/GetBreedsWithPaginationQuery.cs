namespace PetProject.Application.Volunteers.Breeds.Queries
{
    public record GetBreedsWithPaginationQuery(
        Guid SpeciesId,
        int Page,
        int PageSize);
}
