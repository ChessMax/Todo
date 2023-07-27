using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TodoApi.Swagger;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description)
            );
        }

        // TODO: combine swaggers
        // options.SwaggerDoc("all", CreateVersionInfo("0"));
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        return CreateVersionInfo(description.ApiVersion.ToString(), description.IsDeprecated);
    }

    private OpenApiInfo CreateVersionInfo(string version, bool isDeprecated = false)
    {
        var info = new OpenApiInfo
        {
            Title = "Todo API",
            Version = version,
            Description = $"Todo API v{version} Description",
        };

        if (isDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}