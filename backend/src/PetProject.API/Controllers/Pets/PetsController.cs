using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Pets.Requests;
using PetProject.API.Response;
using PetProject.Application.Volunteers.Pets.Queries;

namespace PetProject.API.Controllers.Pets
{
    public class PetsController : ApplicationController
    {
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromServices] PetQueryService service,
            [FromQuery] GetPetsWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetPetsWithPaginationQuery(
                request.Page, request.PageSize, request.PositionFrom, request.PositionTo, request.OrderByDesc, request.OrderBy);

            var response = await service.Get(query);

            return Ok(Envelope.Ok(response));
        }

        [HttpGet("dapper")]
        public async Task<ActionResult> GetDapper(
            [FromServices] PetQueryService service,
            [FromQuery] GetPetsWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = new GetPetsWithPaginationQuery(
                request.Page, request.PageSize, request.PositionFrom, request.PositionTo, request.OrderByDesc, request.OrderBy);

            var response = await service.GetWithDapper(query);

            return Ok(Envelope.Ok(response));
        }
    }
}
