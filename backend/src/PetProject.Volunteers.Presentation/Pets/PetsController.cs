using Microsoft.AspNetCore.Mvc;
using PetProject.Framework;
using PetProject.Volunteers.Application.Pets.Commands.Get;
using PetProject.Volunteers.Application.Pets.Queries;
using PetProject.Volunteers.Presentation.Pets.Requests;

namespace PetProject.Volunteers.Presentation.Pets
{
    public class PetsController : ApplicationController
    {
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromServices] GetPetsWithPaginationHandler handler,
            [FromQuery] GetPetsWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetPetsWithPaginationQuery(
                request.Page, request.PageSize, request.PositionFrom, request.PositionTo, request.OrderByDesc, request.OrderBy);

            var responseResult = await handler.Handle(query, cancellationToken);

            if (responseResult.IsFailure)
                return responseResult.Error.ToResponse();

            return Ok(Envelope.Ok(responseResult.Value));
        }

        [HttpGet("{petId:guid}")]
        public async Task<ActionResult> GetById(
            [FromServices] GetPetHandler handler,
            [FromRoute] Guid petId,
            CancellationToken cancellationToken = default)
        {
            var petResult = await handler.Handle(petId, cancellationToken);

            if (petResult.IsFailure)
                return petResult.Error.ToResponse();

            return Ok(Envelope.Ok(petResult.Value));
        }

        [HttpGet("dapper")]
        public async Task<ActionResult> GetDapper(
            [FromServices] GetPetsWithPaginationDapperHandler handler,
            [FromQuery] GetPetsWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetPetsWithPaginationQuery(
                request.Page, request.PageSize, request.PositionFrom, request.PositionTo, request.OrderByDesc, request.OrderBy);

            var response = await handler.Handle(query, cancellationToken);

            return Ok(Envelope.Ok(response));
        }
    }
}
