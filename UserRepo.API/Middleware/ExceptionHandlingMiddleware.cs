using System.Net;
using System.Text.Json;
using UserRepo.Contracts.Common;

namespace UserRepo.API.Middleware;

/// <summary>
/// Global exception handler. 
/// Catches any uncaught exceptions and converts them into a standardized JSON response.
/// This prevents leaking internal stack traces to the client and keeps responses consistent.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the full exception for developers to see in the logs
            _logger.LogError(ex, "An unhandled exception has occurred.");
            
            // Send a clean, structured error to the user
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500 status code

        var response = new ErrorResponse(
            "InternalServerError", 
            "An unexpected error occurred.", 
            exception.Message // In a production app, you might want to hide this message
        );

        // Serialize the error object to JSON and write to the response stream
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

