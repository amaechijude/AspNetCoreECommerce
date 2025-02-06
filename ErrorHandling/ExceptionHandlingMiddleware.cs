using System.Net;
using System.Text.Json;

namespace AspNetCoreEcommerce.ErrorHandling
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = GlobalConstants.httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var respose = new ErroResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Timestamp = DateTimeOffset.UtcNow
                };

                var jsonResponse = JsonSerializer.Serialize(respose);

                await context.Response.WriteAsync(jsonResponse);
                return;
                
            }

        }
    }

    public class ErroResponse
    {
        public int StatusCode {get; set;}
        public bool Success {get; set;} = false;
        public string? Message {get; set;}
        public DateTimeOffset Timestamp {get; set;}
    }
}