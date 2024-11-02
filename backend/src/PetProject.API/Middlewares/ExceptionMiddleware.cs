using System.Net;
using PetProject.Core;
using PetProject.Framework;

namespace PetProject.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) 
        { 
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext content)
        {
            try
            {
                await _next(content);
            }
            catch (Exception ex)
            {
                Error error = Error.Failure("server.internal", ex.Message);
                Envelope envelope = Envelope.Error(error);

                content.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await content.Response.WriteAsJsonAsync(envelope);

                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
