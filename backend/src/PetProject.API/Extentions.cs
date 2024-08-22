using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Domain.Shared;

namespace PetProject.API
{
    public static class Extentions
    {
        public static ActionResult ToResponse(this Error error)
        {
            int statusCode = default;

            switch (error.ErrorType)
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

            ResponseError responseError = new ResponseError(error.Code, error.Message, null);
            Envelope envelope = Envelope.Error([responseError]);

            return new ObjectResult(envelope) { StatusCode = statusCode };
        }
    }
}
