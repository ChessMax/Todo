using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Services;

namespace TodoApi.Authentification;

public static class AuthentificationExtension
{
    public static IServiceCollection AddAppAuthentification(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<ITokenCreationService, TokenCreationService>();
        services.AddScoped<IApiKeyCreationService, ApiKeyCreationService>();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
                    ),
                };
                // without this option nameidentifier claim is wrong for some reason
                options.MapInboundClaims = false;
            }).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                "ApiKey", options => { });
        return services;
    }
}