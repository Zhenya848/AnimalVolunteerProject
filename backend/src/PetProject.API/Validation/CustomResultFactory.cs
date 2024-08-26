using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetProject.API.Response;
using PetProject.Domain.Shared;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace PetProject.API.Validation
{
    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(
            ActionExecutingContext context, 
            ValidationProblemDetails? validationProblemDetails)
        {
            if (validationProblemDetails != null)
            {
                List<ResponseError> responseErrors = new List<ResponseError>();

                foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
                {
                    Error error = Error.Deserialize(validationErrors[0]);
                    ResponseError responseError = new ResponseError(error.Code, error.Message, invalidField);

                    responseErrors.Add(responseError);
                }

                Envelope envelope = Envelope.Error(responseErrors);

                return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
            }

            throw new InvalidOperationException("Invalid validation problem details");
        }
    }
}
