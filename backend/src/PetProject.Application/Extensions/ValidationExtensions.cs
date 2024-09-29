using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Extensions
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
