using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Species.Requests;
using PetProject.API.Response;
using PetProject.Application.Volunteers.Specieses.Commands;
using PetProject.Application.Volunteers.Specieses.Queries;

namespace PetProject.API.Controllers.Species
{
    public class SpeciesController : ApplicationController
    {
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromServices] GetSpeciesWithPaginationHandler handler,
            [FromQuery] GetSpeciesWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetSpeciesWithPaginationQuery(request.Page, request.PageSize);

            var response = await handler.Get(query, cancellationToken);

            return Ok(Envelope.Ok(response));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] DeleteSpeciesHandler handler,
            CancellationToken cancellationToken = default)
        {
            DeleteSpeciesCommand command = new DeleteSpeciesCommand(id);

            var result = await handler.Delete(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        } 
    }
}
