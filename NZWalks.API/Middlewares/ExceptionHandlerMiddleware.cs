using System.Net;

namespace NZWalks.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware>logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var errorId = Guid.NewGuid();
            // Log the exception
            _logger.LogError(exception,$"{errorId} : {exception.Message}");
            // Return the exception as a response
            context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var error = new
            {
                ErrorId = errorId,
                EroorMessage = "Something went wrong! We are looking into it."
            };
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}