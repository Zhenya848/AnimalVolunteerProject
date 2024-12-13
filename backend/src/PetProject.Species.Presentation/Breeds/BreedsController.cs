using Microsoft.AspNetCore.Mvc;
using PetProject.Framework;
using PetProject.Species.Application.Breeds.Commands;
using PetProject.Species.Application.Breeds.Queries;
using PetProject.Species.Presentation.Breeds.Requests;

namespace PetProject.Species.Presentation.Breeds
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
