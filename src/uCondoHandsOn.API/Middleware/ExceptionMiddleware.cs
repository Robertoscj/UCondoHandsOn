using System.Net;
using System.Text.Json;
using FluentValidation;

namespace uCondo.Planos.API.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    ValidationException => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                _logger.LogError(error, error.Message);

                var result = JsonSerializer.Serialize(new { message = error.Message });
                await response.WriteAsync(result);
            }
        }
    }
}