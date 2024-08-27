using PetProject.API.Response;
using PetProject.Domain.Shared;
using System.Net;

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
                ResponseError responseError = new ResponseError("server.internal", ex.Message, null);
                Envelope envelope = Envelope.Error([responseError]);

                content.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await content.Response.WriteAsJsonAsync(envelope);

                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
