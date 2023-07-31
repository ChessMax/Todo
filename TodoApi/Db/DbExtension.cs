using Microsoft.EntityFrameworkCore;

namespace TodoApi.Db;

public static class DbExtension
{
    public static IServiceCollection AddAppDbContext(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<TodoContext>(options => options.UseSqlite(connectionString));
        return services;
    }
}