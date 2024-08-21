using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Domain.Shared;

namespace PetProject.API
{
    public static class Extentions
    {
        public static ActionResult ToResponse<T>(this Result<T, Error> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(Envelope.Ok(result.Value));

            int statusCode = default;

            switch (result.Error.ErrorType)
            {
                case ErrorType.Validation:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case ErrorType.NotFound:
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case ErrorType.Conflict:
                    statusCode = StatusCodes.Status409Conflict;
                    break;
                case ErrorType.Failure:
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            return new ObjectResult(Envelope.Error(result.Error)) { StatusCode = statusCode };
        }
    }
}
