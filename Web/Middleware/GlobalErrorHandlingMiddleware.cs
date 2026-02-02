using Domain.Exceptions.AlreadyTaken;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Domain.Exceptions.ServerError;
using Domain.Exceptions.UnauthorizedException;
using Shared.ErrorModels;

namespace Web.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if(context.Response.StatusCode == 404)
                {
                    context.Response.ContentType = "application/json";
                    var ResponseBody = new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        ErrorMessage = "This Route Not Match Any EndPoint In Server Side !"
                    };
                    await context.Response.WriteAsJsonAsync(ResponseBody);
                }
            }
            catch(Exception exception)
            {

                context.Response.StatusCode = exception switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    UnauthorizedException => StatusCodes.Status401Unauthorized,
                    AlreadyTakenException => StatusCodes.Status409Conflict,
                    ServerErrorExceptionText => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.ContentType = "application/json";

                var ResponseBody = new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = exception.Message
                };

                await context.Response.WriteAsJsonAsync(ResponseBody);
            }
        }



    }
}
