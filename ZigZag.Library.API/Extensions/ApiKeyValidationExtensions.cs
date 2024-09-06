namespace ZigZag.Library.API.Extensions;

public static class ApiKeyValidationExtensions
{
    public static IApplicationBuilder UseApiKeyValidation(this IApplicationBuilder app, IConfiguration configuration)
    {
        return app.Use(async (context, next) =>
        {
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            var apiKey = configuration.GetValue<string>("ApiKey");

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            await next();
        });
    }
}