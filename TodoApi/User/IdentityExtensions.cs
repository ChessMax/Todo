using Microsoft.AspNetCore.Identity;
using TodoApi.Db;
using TodoApi.User.Domain;

namespace TodoApi.User;

public static class IdentityExtensions
{
    public static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
        services
            .AddIdentityCore<TodoUser>(
                options => { options.SignIn.RequireConfirmedAccount = false; })
            .AddEntityFrameworkStores<TodoContext>();
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        });
        return services;
    }
}