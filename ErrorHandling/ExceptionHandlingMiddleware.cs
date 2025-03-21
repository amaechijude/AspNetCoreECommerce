using System.Net;
using System.Text.Json;
using AspNetCoreEcommerce.Repositories.Implementations;
using static AspNetCoreEcommerce.Repositories.Implementations.VendorRepository;
using static AspNetCoreEcommerce.Services.Implementations.OrderSevice;
namespace AspNetCoreEcommerce.ErrorHandling
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private readonly string httpContentType = GlobalConstants.httpContentType;

        public async Task InvokeAsync(HttpContext context)
        {
            try 
            {
                await _next(context);
                return;
            }
            catch (KeyNotFoundException ex)
            {
                context.Response.ContentType = httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var errorResponse = new 
                {
                    code = (int)HttpStatusCode.NotFound,
                    status = "failed",
                    message = $"{ex.Message}"
                };

                var jsonRespose = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonRespose);
                
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.ContentType = httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                var errorResponse = new 
                {
                    code = (int)HttpStatusCode.Unauthorized,
                    status = "failed",
                    message = $"{ex.Message}"
                };

                var jsonRespose = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonRespose);

                return;
            }
            catch (InvalidOperationException ex)
            {
                context.Response.ContentType = httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errorResponse = new 
                {
                    code = (int)HttpStatusCode.BadRequest,
                    status = "failed",
                    message = $"{ex.Message}"
                };

                var jsonRespose = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonRespose);

                return;
            }
            
            catch (ArgumentException ex)
            {
                context.Response.ContentType = httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errorResponse = new 
                {
                    code = (int)HttpStatusCode.BadRequest,
                    status = "failed",
                    message = $"{ex.Message}"
                };

                var jsonRespose = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonRespose);
                
                return;
            }
            catch(DuplicateException ex)
            {
                context.Response.ContentType = httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errorResponse = new 
                {
                    code = (int)HttpStatusCode.BadRequest,
                    status = "failed",
                    message = $"{ex.Message}"
                };

                var jsonRespose = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonRespose);
                
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