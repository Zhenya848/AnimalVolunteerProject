using FluentValidation.Results;

namespace PetProject.Core.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static ErrorList ValidationErrorResponse(this ValidationResult validationResult)
        {
            var validationErrors = validationResult.Errors;
            List<Error> responseErrors = new List<Error>();

            foreach (var validationError in validationErrors)
            {
                Error error = Error.Deserialize(validationError.ErrorMessage);
                Error responseError = Error.Validation(error.Code, error.Message, error.InvalidField);

                responseErrors.Add(responseError);
            }

            return responseErrors;
        }
    }
}
