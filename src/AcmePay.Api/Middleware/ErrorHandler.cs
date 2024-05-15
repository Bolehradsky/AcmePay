using _Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace AcmePay.Api.Midleware
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    BusinessRuleValidationException => (int)HttpStatusCode.BadRequest,
                    DatabaseException => (int)HttpStatusCode.InternalServerError,
                    EntityNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };
                var result = JsonSerializer.Serialize(new
                {
                    message = error?.Message,
                    type = error?.GetType().FullName,
                    status = response.StatusCode,
                    path = context.Request.Path.Value,
                    occurredat = DateTime.UtcNow,
                });
                await response.WriteAsync(result);
            }
        }
    }
}
