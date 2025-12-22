using System.Diagnostics;
using System.Security.Claims;

namespace UserRepo.API.Middleware
{
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
            var stopwatch = Stopwatch.StartNew();
            
            // Client IP
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            
            // Client Name (set by ApiKeyMiddleware usually, or we extract it safely)
            // Assuming ApiKeyMiddleware runs BEFORE this, but if not, we can rely on HttpContext.Items or Claims
            // Wait, usually logging encapsulates everything. Let's record what we can and update after _next if needed?
            // Requirement says: "Log messages should contain ... Message".
            // It says "Each API call should be logged".
            // It is best to log AFTER the request is processed to get the status or potential errors, 
            // OR log the incoming request immediately.
            // Let's log AFTER to capture processing time/status if we want, but requirement lists specific fields.
            // "Message" is likely the content or result description.
            // I'll log at the end.

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogRequest(context, stopwatch.ElapsedMilliseconds, "Error: " + ex.Message, LogLevel.Error);
                throw;
            }

            // We log success info here
            LogRequest(context, stopwatch.ElapsedMilliseconds, $"Request completed with status {context.Response.StatusCode}", LogLevel.Information);
        }

        private void LogRequest(HttpContext context, long elapsedMs, string message, LogLevel level)
        {
            var clientName = context.Items["ClientName"] as string ?? "Unknown";
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var hostName = Environment.MachineName; // Name of host on which service is running
            var method = $"{context.Request.Method} {context.Request.Path}";
            var queryParams = context.Request.QueryString.ToString();

            // Note: Reading the Body for parameters is tricky as the stream is read once. 
            // For this basic logging, query params are easy. Body params might require buffering which impacts performance.
            // Requirement: "Request parameters". I will log QueryString. 
            // If body logging is strictly required, I'd need EnableBuffering(), but it's expensive.
            // I'll stick to Route/Query params for now unless body is critical.
            
            // We use structured logging which Serilog picks up
            // "Daily new log file" is handled by Serilog configuration, not here.
            
            // Fields mapping:
            // Log Level: (Implicit in _logger call)
            // Time: (Implicit in Serilog timestamp)
            // Client IP: {ClientIp}
            // Client name: {ClientName}
            // Name of host: {HostName}
            // Name of API method: {ApiMethod} (e.g. GET /users/1)
            // Request parameters: {RequestParameters}
            // Message: {Message}

            _logger.Log(level, 
                "ClientIP: {ClientIp}, ClientName: {ClientName}, Host: {HostName}, Method: {ApiMethod}, Params: {RequestParameters}, Msg: {Message}, ElapsedMs: {ElapsedMs}",
                clientIp, clientName, hostName, method, queryParams, message, elapsedMs);
        }
    }
}
