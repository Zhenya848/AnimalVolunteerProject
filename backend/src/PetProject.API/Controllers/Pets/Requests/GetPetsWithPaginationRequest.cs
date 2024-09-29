namespace PetProject.API.Controllers.Pets.Requests
{
    public record GetPetsWithPaginationRequest(int Page, int PageSize);
}
