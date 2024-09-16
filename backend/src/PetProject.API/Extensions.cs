using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.API.Response;
using PetProject.Domain.Shared;
using PetProject.Infastructure;

namespace PetProject.API
{
    public static class Extensions
    {
        public static ActionResult ToResponse(this Error error)
        {
            Envelope envelope = Envelope.Error((ErrorList)error);

            return new ObjectResult(envelope) { StatusCode = GetStatusCode(error) };
        }

        public static ActionResult ToResponse(this ErrorList errors)
        {
            if (errors.Any() == false)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status500InternalServerError };

            Envelope envelope = Envelope.Error(errors);

            return new ObjectResult(envelope) { StatusCode = GetStatusCode(errors) };
        }

        public async static Task ApplyMigrations(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        private static int GetStatusCode(ErrorList errors)
        {
            var distinctErrorTypes = errors.Select(e => e.ErrorType).Distinct().ToList();
            var statusCode = distinctErrorTypes.Count > 1
                ? StatusCodes.Status500InternalServerError
                : distinctErrorTypes.First() switch
                {
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    ErrorType.Required => StatusCodes.Status400BadRequest,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    ErrorType.Failure => StatusCodes.Status500InternalServerError,
                    _ => throw new NotImplementedException()
                };

            return statusCode;
        }
    }
}
