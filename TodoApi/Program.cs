using TodoApi;
using TodoApi.Authentification;
using TodoApi.Swagger.Extensions;
using TodoApi.User;
using TodoApi.Versioning;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddAppDbContext(configuration);
builder.Services.AddAppIdentity();
builder.Services.AddAppAuthentification(configuration);

builder.Services.AddControllers();
builder.Services.AddAppVersioning();
builder.Services.AddOpenApi();
// TODO: AddEndpointsApiExplorer is required only for minimal apis
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();