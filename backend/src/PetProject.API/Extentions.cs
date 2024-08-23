using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public static ActionResult ValidationErrorResponse(this ValidationResult validatorResult)
        {
            if (validatorResult.IsValid == false)
            {
                var validationErrors = validatorResult.Errors;
                List<ResponseError> responseErrors = new List<ResponseError>();

                foreach (var validationError in validationErrors)
                {
                    Error error = Error.Deserialize(validationError.ErrorMessage);
                    ResponseError responseError = new ResponseError(error.Code, error.Message, validationError.PropertyName);

                    responseErrors.Add(responseError);
                }

                return new ObjectResult(Envelope.Error(responseErrors)) { StatusCode = StatusCodes.Status400BadRequest };
            }

            throw new InvalidOperationException("Result can not be succees!");
        }
    }
}
