using GalacticRoutesAPI.Exceptions;
using GalacticRoutesAPI.Managers;
using GalacticRoutesAPI.Models;

namespace GalacticRoutesAPI.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ApiKeyManager apiKeyManager,
            RequestManager requestManager,
            SpaceTravelerManager spaceTravelerManager
            )
        {
            if (!context.Request.Headers.TryGetValue("ApiKey", out var apiKeyHeaderValue) && string.IsNullOrWhiteSpace(apiKeyHeaderValue))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            try
            {
                if (!apiKeyManager.ValidateApiKey(apiKeyHeaderValue))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid API Key.");
                    return;
                }
            }
            catch (RateLimitExceededException ex)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded for cadets.");
                return;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occured validating the Api Key.");
                return;
            }

            // Get the SpaceTraveler who owns the key
            SpaceTraveler requester = spaceTravelerManager.GetWithApiKeyValue(apiKeyHeaderValue);
            // Create a new request entry
            var request = new Request();
            requester.Requests.Add(request);

            await _next(context);
        }
    }
}

