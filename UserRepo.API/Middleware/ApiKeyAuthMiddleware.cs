using System.Security.Claims;
using UserRepo.Domain.Interfaces;

namespace UserRepo.API.Middleware
{
    /// <summary>
    /// Middleware to handle API Key authentication.
    /// Intercepts every request and checks for a valid 'ApiKey' header.
    /// </summary>
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        
        // The name of the header that contains the API key in each request.
        private const string ApiKeyHeaderName = "ApiKey";

        public ApiKeyAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IClientRepository clientRepository)
        {
            // 1. Check if the header exists
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            // 2. Validate the API Key against the database
            var client = await clientRepository.GetByApiKeyAsync(extractedApiKey.ToString());

            if (client == null || !client.IsActive)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            // 3. Store ClientName in HttpContext.Items so other middleware (like Logging) can access it.
            context.Items["ClientName"] = client.Name;

            // 4. Continue to the next piece of middleware in the pipeline
            await _next(context);
        }
    }
}
