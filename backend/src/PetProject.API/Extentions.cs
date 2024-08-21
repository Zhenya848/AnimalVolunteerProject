using Microsoft.AspNetCore.Mvc;
using PetProject.Domain.Shared;

namespace PetProject.API
{
    public static class Extentions
    {
        public static ActionResult ToErrorResponse(this Error error)
        {
            int statusCode = StatusCodes.Status400BadRequest;

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
                case ErrorType.ValueIsRequired:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
            }

            return new ObjectResult(error) { StatusCode = statusCode };
        }
    }
}
