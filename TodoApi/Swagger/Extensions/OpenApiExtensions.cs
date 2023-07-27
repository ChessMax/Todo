using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace TodoApi.Swagger.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at
        // https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert ApiKey with Api-Key into field",
                Name = "Api-Key",
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            // This call remove version from parameter, without it we will have version as parameter 
            // for all endpoints in swagger UI
            // options.OperationFilter<RemoveVersionFromParameter>();
            // This make replacement of v{version:apiVersion} to real version of corresponding swagger doc.
            options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                    },
                    Array.Empty<string>()
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    Array.Empty<string>()
                }
            });

            var filePath = Path.Combine(AppContext.BaseDirectory, "TodoApi.xml");
            options.IncludeXmlComments(filePath);

            // options.ResolveConflictingActions(descriptions => descriptions.First());
        });
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        return services;
    }

    public static WebApplication UseOpenApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetService<IApiVersionDescriptionProvider>()!;
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    $"Todo API {description.GroupName}"
                );
            }
            // TODO: combine swaggers
            //options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API");
        });
        return app;
    }
}