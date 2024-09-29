using Microsoft.AspNetCore.Mvc;
using PetProject.Application.Volunteers.Queries.Pets.Services;
using PetProject.API.Controllers.Pets.Requests;
using PetProject.Application.Volunteers.Queries.Pets;
using PetProject.API.Response;

namespace PetProject.API.Controllers.Pets
{
    public class PetsController : ApplicationController
    {
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromServices] IPetQueryService service,
            [FromQuery] GetPetsWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetPetsWithPaginationQuery(request.Page, request.PageSize);
            var response = await service.Get(query, cancellationToken);

            return Ok(Envelope.Ok(response));
        }
    }
}
