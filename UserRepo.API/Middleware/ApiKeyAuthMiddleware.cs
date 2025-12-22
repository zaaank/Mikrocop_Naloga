using System.Security.Claims;
using UserRepo.Domain.Interfaces;

namespace UserRepo.API.Middleware
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        //The name of the header that contains the API key in each request
        private const string ApiKeyHeaderName = "ApiKey";

        public ApiKeyAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IClientRepository clientRepository)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            var client = await clientRepository.GetByApiKeyAsync(extractedApiKey.ToString());

            if (client == null || !client.IsActive)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            // Store ClientName in HttpContext.Items so LoggingMiddleware can read it
            context.Items["ClientName"] = client.Name;

            await _next(context);
        }
    }
}
