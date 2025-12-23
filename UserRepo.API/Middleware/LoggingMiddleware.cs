using System.Diagnostics;
using System.Security.Claims;

namespace UserRepo.API.Middleware
{
    /// <summary>
    /// Middleware to log every incoming API request and its outcome.
    /// Captures Client IP, Client Name (from Auth middleware), processing time, and status.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Start a timer to measure how long the request takes
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Call the next middleware in the pipeline (the actual API logic)
                await _next(context);
                
                // If successful, log as Information
                LogRequest(context, stopwatch.ElapsedMilliseconds, $"Request completed with status {context.Response.StatusCode}", LogLevel.Information);
            }
            catch (Exception ex)
            {
                // If an unhandled exception occurred, log as Error
                LogRequest(context, stopwatch.ElapsedMilliseconds, "Error: " + ex.Message, LogLevel.Error);
                throw; // Rethrow so the global exception handler can catch it
            }
        }

        /// <summary>
        /// Formats and writes the log message using structured logging.
        /// </summary>
        private void LogRequest(HttpContext context, long elapsedMs, string message, LogLevel level)
        {
            // Gather all required fields for the log
            var clientName = context.Items["ClientName"] as string ?? "Unknown";
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var hostName = Environment.MachineName; // Name of the server/machine running the API
            var method = $"{context.Request.Method} {context.Request.Path}";
            var queryParams = context.Request.QueryString.ToString();

            // We use structured logging which allows easy searching in log management tools (like Seq, ELK, etc.)
            // The actual sink (File) is configured in appsettings.json via Serilog.
            _logger.Log(level, 
                "ClientIP: {ClientIp}, ClientName: {ClientName}, Host: {HostName}, Method: {ApiMethod}, Params: {RequestParameters}, Msg: {Message}, ElapsedMs: {ElapsedMs}",
                clientIp, clientName, hostName, method, queryParams, message, elapsedMs);
        }
    }
}
