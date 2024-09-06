using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ZigZag.Library.API.Extensions;

public static class SwaggerGenOptionsExtensions
{
    public static void AddApiKeySupport(this SwaggerGenOptions options)
    {
        var apiKeyScheme = new OpenApiSecurityScheme
        {
            Description = "API Key required. Add 'X-Api-Key' to request headers.",
            In = ParameterLocation.Header,
            Name = "X-Api-Key",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "ApiKeyScheme"
        };

        options.AddSecurityDefinition("ApiKey", apiKeyScheme);

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { apiKeyScheme, new List<string>() }
        });
    }
}