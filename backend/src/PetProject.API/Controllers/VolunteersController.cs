using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Application.Volunteers.Create;
using PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService;
using PetProject.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PetProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolunteersController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] IVolunteerService volunteerService,
            [FromServices] IValidator<CreateVolunteerRequest> validator,
            [FromBody] CreateVolunteerRequest createVolunteerRequest, 
            CancellationToken cancellationToken = default)
        {
            var validatorResult = await validator.ValidateAsync(createVolunteerRequest, cancellationToken);

            if (validatorResult.IsValid == false)
            {
                var validationErrors = validatorResult.Errors;

                List<ResponseError> responseErrors = 
                    (from validationError in validationErrors                                 
                     let error = Error.Validation(validationError.ErrorCode, validationError.ErrorMessage)
                     select new ResponseError(error.Code, error.Message, validationError.PropertyName)).ToList();

                return BadRequest(Envelope.Error(responseErrors));
            }

            var result = await volunteerService.Create(createVolunteerRequest, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Created("", Envelope.Ok(result.Value).ToString());
        }
    }
}