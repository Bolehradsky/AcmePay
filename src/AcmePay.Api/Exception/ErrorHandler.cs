using _Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace AcmePay.Api.Exception
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
            catch (System.Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    BusinessRuleValidationException => (int)HttpStatusCode.BadRequest,

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
