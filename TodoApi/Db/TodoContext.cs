using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Authentication.Data;
using TodoApi.Items.Domain;
using TodoApi.User.Domain;

namespace TodoApi.Db;

// TODO: check IdentityDbContext
public class TodoContext : IdentityUserContext<TodoUser>
{
    public required DbSet<TodoItem> Items { get; set; }
    public required DbSet<UserApiKey> UserApiKeys { get; set; }

    public TodoContext(DbContextOptions<TodoContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TodoUser>().HasQueryFilter(user => !user.IsDeleted);
    }
}