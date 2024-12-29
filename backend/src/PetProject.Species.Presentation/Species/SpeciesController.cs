using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.Framework;
using PetProject.Framework.Authorization;
using PetProject.Species.Application.Breeds.Commands;
using PetProject.Species.Application.Breeds.Commands.Create;
using PetProject.Species.Application.Species.Commands.Create;
using PetProject.Species.Application.Species.Commands.Delete;
using PetProject.Species.Application.Species.Queries;
using PetProject.Species.Presentation.Species.Requests;

namespace PetProject.Species.Presentation.Species
{
    public class SpeciesController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] CreateSpeciesHandler handler,
            [FromQuery] string name,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateSpeciesCommand(name);
            
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();
            
            return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
        }
        
        [HttpGet]
        //[Permission("species.get")]
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
        //[Permission("species.delete")]
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

        [HttpPost("{speciesId:guid}")]
        public async Task<ActionResult> CreateBreed(
            [FromServices] CreateBreedHandler handler,
            [FromRoute] Guid speciesId,
            [FromQuery] string name,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateBreedCommand(speciesId, name);
            
            var result = await handler.Handle(command, cancellationToken);
            
            if (result.IsFailure)
                return result.Error.ToResponse();
            
            return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
