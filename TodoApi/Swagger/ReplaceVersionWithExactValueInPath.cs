using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TodoApi.Swagger;

public class ReplaceVersionWithExactValueInPath : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = new OpenApiPaths(swaggerDoc.Paths);
        foreach (var (key, value) in swaggerDoc.Paths)
        {
            paths[key.Replace("v{version}", swaggerDoc.Info.Version)] = value;
        }
        swaggerDoc.Paths = paths;
    }
}