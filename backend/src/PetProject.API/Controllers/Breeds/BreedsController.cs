using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Breeds.Requests;
using PetProject.API.Controllers.Species.Requests;
using PetProject.API.Response;
using PetProject.Application.Volunteers.Breeds.Queries;
using PetProject.Application.Volunteers.Specieses.Queries;

namespace PetProject.API.Controllers.Breeds
{
    public class BreedsController : ApplicationController
    {
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromServices] GetBreedsWithPaginationHandler handler,
            [FromQuery] GetBreedsWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetBreedsWithPaginationQuery(request.SpeciesId, request.Page, request.PageSize);

            var response = await handler.Handle(query, cancellationToken);

            return Ok(Envelope.Ok(response));
        }
    }
}
