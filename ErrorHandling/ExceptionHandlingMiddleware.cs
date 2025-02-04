using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetCoreEcommerce.ErrorHandling
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private readonly JsonSerializerOptions _jsonOption = new() { PropertyNameCaseInsensitive = true };

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
                    Timestamp = DateTime.UtcNow
                };

                var jsonResponse = JsonSerializer.Serialize(respose, _jsonOption);

                await context.Response.WriteAsJsonAsync(jsonResponse);
                
            }

        }
    }

    public class ErroResponse
    {
        public int StatusCode {get; set;}
        public bool Success {get; set;} = false;
        public string? Message {get; set;}
        public DateTime Timestamp {get; set;}
    }
}