using GalacticRoutesAPI.Exceptions;
using GalacticRoutesAPI.Managers;
using GalacticRoutesAPI.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GalacticRoutesAPI.Middleware
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiKeyAttribute : ActionFilterAttribute
    {
        //private readonly ApiKeyManager apiKeyManager;
        //private readonly RequestManager requestManager;
        //private readonly SpaceTravelerManager spaceTravelerManager;

        //public ApiKeyAttribute(
        //    ApiKeyManager apiKeyManager,
        //    RequestManager requestManager,
        //    SpaceTravelerManager spaceTravelerManager)
        //{
        //    this.apiKeyManager = apiKeyManager;
        //    this.requestManager = requestManager;
        //    this.spaceTravelerManager = spaceTravelerManager;
        //}

        public async override Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
            )
        {
            var services = context.HttpContext.RequestServices;

            if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var apiKeyHeaderValue) && string.IsNullOrWhiteSpace(apiKeyHeaderValue))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.HttpContext.Response.WriteAsync("API Key is missing.").Wait();
                return;
            }

            try
            {
                ApiKeyManager apiKeyManager = services.GetService<ApiKeyManager>();
                if (!apiKeyManager.ValidateApiKey(apiKeyHeaderValue))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.HttpContext.Response.WriteAsync("Invalid API Key.");
                    return;
                }
            }
            catch (RateLimitExceededException ex)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync("Rate limit exceeded for cadets.");
                return;
            }
            catch (Exception ex)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.HttpContext.Response.WriteAsync("An error occured validating the Api Key.");
                return;
            }

            // Get the SpaceTraveler who owns the key
            SpaceTravelerManager spaceTravelerManager = services.GetService<SpaceTravelerManager>();
            SpaceTraveler requester = spaceTravelerManager.GetWithApiKeyValue(apiKeyHeaderValue);
            // Create a new request entry
            RequestManager requestManager = services.GetService<RequestManager>();
            var request = new Request();
            requester.Requests.Add(request);

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
