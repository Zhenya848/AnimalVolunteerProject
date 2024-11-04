using Microsoft.AspNetCore.Mvc;
using PetProject.Framework;
using PetProject.Framework.Authorization;
using PetProject.Species.Application.Commands;
using PetProject.Species.Application.Queries;
using PetProject.Species.Presentation.Species.Requests;

namespace PetProject.Species.Presentation.Species
{
    public class SpeciesController : ApplicationController
    {
        [HttpGet]
        [Permission("species.get")]
        public async Task<ActionResult> Get(
            [FromServices] GetSpeciesWithPaginationHandler handler,
            [FromQuery] GetSpeciesWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetSpeciesWithPaginationQuery(request.Page, request.PageSize);

            var response = await handler.Handle(query, cancellationToken);

            return Ok(Envelope.Ok(response));
        }

        [HttpDelete("{id:guid}")]
        [Permission("species.delete")]
        public async Task<ActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] DeleteSpeciesHandler handler,
            CancellationToken cancellationToken = default)
        {
            DeleteSpeciesCommand command = new DeleteSpeciesCommand(id);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        } 
    }
}
